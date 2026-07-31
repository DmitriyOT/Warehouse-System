using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Infrastructure;
using Warehouse.Domain.Models;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Tests.TestInfrastructure;

public class TestBalanceRepository : TestCrudRepository<BalanceEntity>, IBalanceRepository
{

    public TestBalanceRepository() : base()
    {
    }

    public Task<BalanceEntity?> GetBalanceAsync(long resourceId, long unitId)
    {
        foreach (var item in _entities.Values)
        {
            if(item.ResourceId == resourceId && item.UnitId == unitId)
            {
                return Task.FromResult<BalanceEntity?>(item);
            }
        }
        return Task.FromResult<BalanceEntity?>(null);
    }

    public Task<bool> TryApplyDeltaAsync(long resourceId, long unitId, long delta)
    {
        var item = _entities.Values.FirstOrDefault(x => x.ResourceId == resourceId && x.UnitId == unitId);
        if (item == null)
        {
            return Task.FromResult(false);
        }

        if (delta < 0 && item.Quantity + delta < 0)
        {
            return Task.FromResult(false);
        }

        item.Quantity += delta;
        if (delta < 0 && item.Quantity == 0)
        {
            _entities.Remove(item.Id);
        }

        return Task.FromResult(true);
    }
}
