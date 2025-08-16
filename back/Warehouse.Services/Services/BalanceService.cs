using Warehouse.Contracts.Application;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;
using Warehouse.Contracts.Exceptions;
using Warehouse.Application.Services.Base;

namespace Warehouse.Application.Services;
public class BalanceService : CrudService<BalanceEntity>, IBalanceService
{

    public BalanceService(IBalanceRepository balanceRepository) : base(balanceRepository)
    {
    }

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

    public async Task CalculateAndApplyDifference(ICollection<ShipmentItemEntity> itemsOld, ICollection<ShipmentItemEntity> itemsNow)
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

    //Функция для вычисления и суммирования разницы по товарам
    private ICollection<BalanceItem> CalculateDiff(Dictionary<long, BalanceDiffItem> oldItems, Dictionary<long, BalanceDiffItem> nowItems)
    {
        //Работает с накоплениями, неважно как они представлены в old и now items
        var dic = new Dictionary<Tuple<long, long>, long>(); //resourceId, unitId, count

        //Вспомогательная функция для словаря
        var addDic = (BalanceItem item) =>
        {
            var key = Tuple.Create(item.ResourceId, item.UnitId);
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
                ResourceId = item.Key.Item1,
                UnitId = item.Key.Item2,
                Delta = item.Value
            });
        }

        return result;
    }

    private async Task ApplyDiff(ICollection<BalanceItem> items)
    {
        foreach (var item in items)
        {
            var balance = await (_repository as IBalanceRepository)!.GetBalanceAsync(item.ResourceId, item.UnitId);
            if (balance == null)
            {
                if (item.Delta < 0)
                {
                    throw new UserException("Ошибка. Недостаточно ресурсов на балансе.");
                }

                if (item.Delta == 0)
                {
                    continue;
                }

                balance = new BalanceEntity
                {
                    Id = 0,
                    Quantity = item.Delta,
                    ResourceId = item.ResourceId,
                    UnitId = item.UnitId,
                };
            }
            else
            {
                if(balance.Quantity + item.Delta < 0)
                {
                    throw new UserException("Ошибка. Недостаточно ресурсов на балансе.");
                }

                balance.Quantity += item.Delta;
            }
            
            if (balance.Quantity != 0)
                await (_repository as IBalanceRepository)!.EditItem(balance);
            else
                await (_repository as IBalanceRepository)!.DeleteItem(balance.Id);
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