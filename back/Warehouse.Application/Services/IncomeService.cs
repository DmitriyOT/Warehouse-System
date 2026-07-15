using Warehouse.Application.Services.Base;
using Warehouse.Contracts.Api.Request.Dtos;
using Warehouse.Contracts.Application;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Services;

/// <summary>
/// Сервис для поступлений
/// </summary>
public class IncomeService : CrudService<IncomeEntity>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IBalanceService _balanceService;
    private readonly IUnitOfWork _unitOfWork;

    public IncomeService(IIncomeRepository repository, IBalanceService balanceService, IUnitOfWork unitOfWork) : base(repository)
    {
        _incomeRepository = repository;
        _balanceService = balanceService;
        _unitOfWork = unitOfWork;
    }

    private static void Validate(IncomeEntity item)
    {
        //Проверка на ресурсы
        if (item.IncomeItems != null)
        {
            foreach (var elem in item.IncomeItems)
            {
                if(elem.Quantity <= 0)
                {
                    throw new UserException("Ошибка. Количество ресурса должно быть положительным в документе поступления.");
                }
            }
        }
    }

    /// <summary>
    /// Создать или обновить документ поступления на основе DTO
    /// </summary>
    public async Task<long> EditItem(IncomeEditDto dto)
    {
        var entity = new IncomeEntity
        {
            Id = dto.Id,
            Number = dto.Number,
            Date = dto.Date,
            IncomeItems = dto.IncomeItems.Select(i => new IncomeItemEntity
            {
                Id = i.Id,
                ResourceId = i.ResourceId,
                UnitId = i.UnitId,
                Quantity = i.Quantity
            }).ToList()
        };

        Validate(entity);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            List<IncomeItemEntity>? oldItems = null;
            if (dto.Id > 0)
            {
                var oldEntity = await _incomeRepository.GetItem(dto.Id);
                oldItems = oldEntity.IncomeItems?.ToList();
            }

            var id = await _incomeRepository.EditItem(entity);

            if (oldItems != null && entity.IncomeItems != null)
            {
                await _balanceService.CalculateAndApplyDifference(oldItems, entity.IncomeItems);
            }
            else if (entity.IncomeItems != null)
            {
                await _balanceService.ApplyIncomeDifference(entity.IncomeItems);
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
    /// Удалить документ поступления и скорректировать баланс
    /// </summary>
    public override async Task DeleteItem(long id)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var item = await _incomeRepository.GetItem(id);
            await _incomeRepository.DeleteItem(id);

            if (item.IncomeItems != null)
            {
                foreach (var i in item.IncomeItems)
                {
                    i.Quantity = -i.Quantity;
                }
                await _balanceService.ApplyIncomeDifference(item.IncomeItems);
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
