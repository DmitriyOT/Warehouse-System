using Warehouse.Contracts.Application;
using Warehouse.Contracts.Infrastructure;
using Warehouse.Domain.Models;
using Warehouse.Contracts.Exceptions;
using Warehouse.Application.Services.Base;

namespace Warehouse.Application.Services;

/// <summary>
/// Сервис для работы с балансом (остатками склада)
/// </summary>
public class BalanceService : CrudService<BalanceEntity>, IBalanceService
{
    private readonly IBalanceRepository _balanceRepository;

    public BalanceService(IBalanceRepository balanceRepository) : base(balanceRepository)
    {
        _balanceRepository = balanceRepository;
    }

    /// <summary>
    /// Применить изменения поступления
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public async Task ApplyIncomeDifference(ICollection<IncomeItemEntity> items)
    {
        var arr = items.Select(x => new BalanceItem 
        { 
            ResourceId = x.ResourceId,
            UnitId = x.UnitId,
            Delta = x.Quantity
        }).ToArray();

        await ApplyDiff(arr);
    }

    /// <summary>
    /// Применить изменения отгрузки
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public async Task ApplyShipmentDifference(ICollection<ShipmentItemEntity> items)
    {
        var arr = items.Select(x => new BalanceItem
        {
            ResourceId = x.ResourceId,
            UnitId = x.UnitId,
            Delta = x.Quantity
        }).ToArray();

        await ApplyDiff(arr);
    }

    /// <summary>
    /// Вычислить разницу и применить её к балансу
    /// </summary>
    /// <param name="itemsOld"></param>
    /// <param name="itemsNow"></param>
    /// <returns></returns>
    public async Task CalculateAndApplyDifference(ICollection<IncomeItemEntity> itemsOld, ICollection<IncomeItemEntity> itemsNow)
    {
        var arrOld = itemsOld.Select(x => new BalanceDiffItem
        {
            Id = x.Id,
            ResourceId = x.ResourceId,
            UnitId = x.UnitId,
            Delta = x.Quantity
        }).ToDictionary(x => x.Id);

        var arrNow = itemsNow.Select(x => new BalanceDiffItem
        {
            Id = x.Id,
            ResourceId = x.ResourceId,
            UnitId = x.UnitId,
            Delta = x.Quantity
        }).ToDictionary(x => x.Id);

        await ApplyDiff(CalculateDiff(arrOld, arrNow));
    }

    /// <summary>
    /// Вычислить разницу отгрузки и применить её к балансу (отгрузка уменьшает остаток)
    /// </summary>
    /// <param name="itemsOld"></param>
    /// <param name="itemsNow"></param>
    /// <returns></returns>
    public async Task CalculateAndApplyShipmentDifference(ICollection<ShipmentItemEntity> itemsOld, ICollection<ShipmentItemEntity> itemsNow)
    {
        var arrOld = itemsOld.Select(x => new BalanceDiffItem
        {
            Id = x.Id,
            ResourceId = x.ResourceId,
            UnitId = x.UnitId,
            Delta = -x.Quantity
        }).ToDictionary(x => x.Id);

        var arrNow = itemsNow.Select(x => new BalanceDiffItem
        {
            Id = x.Id,
            ResourceId = x.ResourceId,
            UnitId = x.UnitId,
            Delta = -x.Quantity
        }).ToDictionary(x => x.Id);

        await ApplyDiff(CalculateDiff(arrOld, arrNow));
    }

    //Функция для вычисления и суммирования разницы по товарам
    private ICollection<BalanceItem> CalculateDiff(Dictionary<long, BalanceDiffItem> oldItems, Dictionary<long, BalanceDiffItem> nowItems)
    {
        //Работает с накоплениями, неважно как они представлены в old и now items
        var dic = new Dictionary<(long ResourceId, long UnitId), long>(); //resourceId, unitId, count

        //Вспомогательная функция для словаря
        var addDic = (BalanceItem item) =>
        {
            var key = (item.ResourceId, item.UnitId);
            if (dic.TryGetValue(key, out long oldValue))
            {
                dic[key] = item.Delta + oldValue;
            }
            else
            {
                dic.Add(key, item.Delta);
            }
        };

        foreach(var item in nowItems.Values)
        {
            if(oldItems.TryGetValue(item.Id, out var oldValue))
            {//Изменённые значения
                if(item.ResourceId == oldValue.ResourceId && item.UnitId == oldValue.UnitId)
                {//Разница только по количеству
                    item.Delta -= oldValue.Delta;
                    addDic(item);
                }
                else
                {//Изменился тип ресурса + ЕИ, старое уменьшить, новое добавить
                    oldValue.Delta = -oldValue.Delta;
                    addDic(oldValue);
                    addDic(item);
                }
            }
            else
            {//Добавленные значения
                addDic(item);
            }
        }

        foreach (var item in oldItems.Values)
        {//Раньше было а сейчас нет, удалённые
            if (!nowItems.ContainsKey(item.Id))
            {
                item.Delta = -item.Delta;
                addDic(item);
            }
        }

        var result = new List<BalanceItem>();

        foreach(var item in dic)
        {
            result.Add(new BalanceItem
            {
                ResourceId = item.Key.ResourceId,
                UnitId = item.Key.UnitId,
                Delta = item.Value
            });
        }

        return result;
    }

    //Применяем разницу к балансу атомарным UPDATE, чтобы параллельные списания не уводили остаток в минус
    private async Task ApplyDiff(ICollection<BalanceItem> items)
    {
        foreach (var item in items)
        {
            if (item.Delta == 0)
            {
                continue;
            }

            var applied = await _balanceRepository.TryApplyDeltaAsync(item.ResourceId, item.UnitId, item.Delta);
            if (applied)
            {
                continue;
            }

            if (item.Delta < 0)
            {//Строки баланса нет или остатка не хватает
                throw new UserException("Ошибка. Недостаточно ресурсов на балансе.");
            }

            //Строки баланса ещё нет — создаём её (только для положительного изменения)
            await _balanceRepository.EditItem(new BalanceEntity
            {
                Id = 0,
                Quantity = item.Delta,
                ResourceId = item.ResourceId,
                UnitId = item.UnitId,
            });
        }
    }

    private class BalanceItem
    {
        public long ResourceId { get; set; }

        public long UnitId { get; set; }

        public long Delta { get; set; }
    }

    private class BalanceDiffItem : BalanceItem
    {
        public long Id { get; set; }
    }
}