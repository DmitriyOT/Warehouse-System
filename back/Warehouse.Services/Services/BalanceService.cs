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

    private async Task ApplyDiff(ICollection<BalanceItem> items)
    {
        foreach (var item in items)
        {
            var balance = await (_repository as IBalanceRepository)!.GetBalanceAsync(item.ResourceId, item.UnitId);
            if(balance == null)
            {
                if(item.Delta < 0)
                {
                    throw new UserException("Ошибка. Недостаточно ресурсов на балансе.");
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
}

internal class BalanceItem
{
    public long ResourceId { get; set; }

    public long UnitId { get; set; }

    public long Delta { get; set; }
}