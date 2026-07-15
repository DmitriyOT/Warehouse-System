using Warehouse.Application.Services.Base;
using Warehouse.Contracts.Api.Request.Dtos;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Services;

/// <summary>
/// Сервис для поступлений
/// </summary>
public class IncomeService : CrudService<IncomeEntity>
{
    public IncomeService(IIncomeRepository repository) : base(repository)
    {
    }

    //Редактирование элемента
    public override Task<long> EditItem(IncomeEntity item)
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

        return base.EditItem(item);
    }

    /// <summary>
    /// Создать или обновить документ поступления на основе DTO
    /// </summary>
    public Task<long> EditItem(IncomeEditDto dto)
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

        return EditItem(entity);
    }
}
