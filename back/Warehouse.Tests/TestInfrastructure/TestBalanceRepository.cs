using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Infrastracture;
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
}
