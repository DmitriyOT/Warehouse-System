using Warehouse.Contracts.Infrastructure;
using Warehouse.Domain.Models;

namespace Warehouse.Tests.TestInfrastructure;

public class TestShipmentRepository : TestCrudRepository<ShipmentEntity>, IShipmentRepository
{
    public TestShipmentRepository() : base()
    {
    }

    public Task ChangeStateAsync(long id, string newStateCode)
    {
        _entities[id].IsApprove = newStateCode == "approve";
        return Task.CompletedTask;
    }
}
