using Warehouse.Application.Services.Base;
using Warehouse.Contracts.Api.Request.Dtos;
using Warehouse.Contracts.Application;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastructure;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Services;

/// <summary>
/// Сервис для отгрузок
/// </summary>
public class ShipmentService : CrudService<ShipmentEntity>
{
    private readonly IShipmentRepository _shipmentRepository;
    private readonly IBalanceService _balanceService;
    private readonly IUnitOfWork _unitOfWork;

    public ShipmentService(IShipmentRepository repository, IBalanceService balanceService, IUnitOfWork unitOfWork) : base(repository)
    {
        _shipmentRepository = repository;
        _balanceService = balanceService;
        _unitOfWork = unitOfWork;
    }

    private static void Validate(ShipmentEntity item)
    {
        //Проверяем бизнес логику ресурсов отгрузки
        if (item.ShipmentItems != null && item.ShipmentItems.Count > 0)
        {
            foreach (var elem in item.ShipmentItems)
            {
                if (elem.Quantity <= 0)
                {
                    throw new UserException("Ошибка. Количество ресурса должно быть положительным в документе отгрузки.");
                }
            }
        }
        else
        {
            throw new UserException("Ошибка. Документ отгрузки должен содержать хотя бы 1 ресурс.");
        }
    }

    /// <summary>
    /// Создать или обновить документ отгрузки на основе DTO
    /// </summary>
    public async Task<long> EditItem(ShipmentEditDto dto)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            ShipmentEntity? oldEntity = null;
            if (dto.Id > 0)
            {
                oldEntity = await _shipmentRepository.GetItem(dto.Id);
            }

            var entity = new ShipmentEntity
            {
                Id = dto.Id,
                Number = dto.Number,
                Date = dto.Date,
                ClientId = dto.ClientId,
                //Состояние подписи меняется только через ChangeStateAsync, при редактировании сохраняем текущее
                IsApprove = oldEntity?.IsApprove ?? false,
                ShipmentItems = dto.ShipmentItems.Select(i => new ShipmentItemEntity
                {
                    Id = i.Id,
                    ResourceId = i.ResourceId,
                    UnitId = i.UnitId,
                    Quantity = i.Quantity
                }).ToList()
            };

            Validate(entity);

            var id = await _shipmentRepository.EditItem(entity);

            //Пересчитываем баланс только для подписанного документа:
            //возвращаем старые количества и списываем новые (отгрузка уменьшает остаток)
            if (oldEntity is { IsApprove: true } && oldEntity.ShipmentItems != null && entity.ShipmentItems != null)
            {
                await _balanceService.CalculateAndApplyShipmentDifference(oldEntity.ShipmentItems, entity.ShipmentItems);
            }

            await transaction.CommitAsync();
            return id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Удалить документ отгрузки и вернуть товар на баланс, если документ был подписан
    /// </summary>
    public override async Task DeleteItem(long id)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var item = await _shipmentRepository.GetItem(id);
            await _shipmentRepository.DeleteItem(id);

            //Подписанная отгрузка списала остаток — возвращаем количества на баланс
            if (item.IsApprove && item.ShipmentItems != null)
            {
                foreach (var i in item.ShipmentItems)
                {
                    i.Quantity = Math.Abs(i.Quantity);
                }
                await _balanceService.ApplyShipmentDifference(item.ShipmentItems);
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    //Изменение состояния, подписание отгрузки
    public async Task ChangeStateAsync(long id, string newStateCode)
    {
        var isApprove = newStateCode == "approve";

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var item = await _shipmentRepository.GetItem(id);
            var wasApprove = item.IsApprove;

            await _shipmentRepository.ChangeStateAsync(id, newStateCode);

            if (item.ShipmentItems != null)
            {
                // Если документ становится подписанным — списываем с баланса (отрицательное количество).
                // Если снимаем подпись — возвращаем на баланс (положительное количество).
                var needApply = (isApprove && !wasApprove) || (!isApprove && wasApprove);
                if (needApply)
                {
                    foreach (var i in item.ShipmentItems)
                    {
                        i.Quantity = isApprove ? -i.Quantity : Math.Abs(i.Quantity);
                    }
                    await _balanceService.ApplyShipmentDifference(item.ShipmentItems);
                }
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
