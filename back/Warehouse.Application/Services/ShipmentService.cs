using Warehouse.Application.Services.Base;
using Warehouse.Contracts.Api.Request.Dtos;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Services;

/// <summary>
/// Сервис для отгрузок
/// </summary>
public class ShipmentService : CrudService<ShipmentEntity>
{
    private readonly IShipmentRepository _shipmentRepository;

    public ShipmentService(IShipmentRepository repository) : base(repository)
    {
        _shipmentRepository = repository;
    }

    public override Task<long> EditItem(ShipmentEntity item)
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

        return base.EditItem(item);
    }

    /// <summary>
    /// Создать или обновить документ отгрузки на основе DTO
    /// </summary>
    public Task<long> EditItem(ShipmentEditDto dto)
    {
        var entity = new ShipmentEntity
        {
            Id = dto.Id,
            Number = dto.Number,
            Date = dto.Date,
            ClientId = dto.ClientId,
            IsApprove = dto.IsApprove,
            ShipmentItems = dto.ShipmentItems.Select(i => new ShipmentItemEntity
            {
                Id = i.Id,
                ResourceId = i.ResourceId,
                UnitId = i.UnitId,
                Quantity = i.Quantity
            }).ToList()
        };

        return EditItem(entity);
    }

    //Изменение состояния, подписание отгрузки
    public Task ChangeStateAsync(long id, string newStateCode)
    {
        return _shipmentRepository.ChangeStateAsync(id, newStateCode);
    }
}
