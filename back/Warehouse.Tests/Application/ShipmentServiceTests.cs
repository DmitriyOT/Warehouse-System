using Warehouse.Application.Services;
using Warehouse.Contracts.Api.Request.Dtos;
using Warehouse.Contracts.Exceptions;
using Warehouse.Domain.Models;
using Warehouse.Tests.TestInfrastructure;

namespace Warehouse.Tests.Application;

public class ShipmentServiceTests
{
    private static ShipmentEntity Shipment(long id, bool isApprove, params ShipmentItemEntity[] items)
        => new ShipmentEntity
        {
            Id = id,
            Number = $"О{id}",
            Date = new DateOnly(2026, 1, 1),
            ClientId = 1,
            IsApprove = isApprove,
            ShipmentItems = items.ToList()
        };

    private static ShipmentEditDto EditDto(long id, params ShipmentItemEditDto[] items)
        => new ShipmentEditDto
        {
            Id = id,
            Number = $"О{id}",
            Date = new DateOnly(2026, 1, 1),
            ClientId = 1,
            ShipmentItems = items.ToList()
        };

    private static async Task<long> BalanceQuantity(TestBalanceRepository balanceRepository)
    {
        var all = await balanceRepository.GetAll(null);
        return all.Items.Sum(x => x.Quantity);
    }

    [Fact]
    public async Task EditItem_ApprovedShipment_RecalculatesBalance()
    {
        var shipmentRepository = new TestShipmentRepository();
        await shipmentRepository.EditItem(Shipment(1, true,
            new ShipmentItemEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 4 }));

        var balanceRepository = new TestBalanceRepository();
        //Остаток уже учитывает списание 4 единиц подписанной отгрузкой
        await balanceRepository.EditItem(new BalanceEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 10 });

        var service = new ShipmentService(shipmentRepository, new BalanceService(balanceRepository), new TestUnitOfWork());

        await service.EditItem(EditDto(1,
            new ShipmentItemEditDto { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 6 }));

        //10 + 4 (возврат старых) - 6 (списание новых) = 8
        Assert.Equal(8, await BalanceQuantity(balanceRepository));
    }

    [Fact]
    public async Task EditItem_UnapprovedShipment_BalanceUntouched()
    {
        var shipmentRepository = new TestShipmentRepository();
        await shipmentRepository.EditItem(Shipment(1, false,
            new ShipmentItemEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 4 }));

        var balanceRepository = new TestBalanceRepository();
        await balanceRepository.EditItem(new BalanceEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 10 });

        var service = new ShipmentService(shipmentRepository, new BalanceService(balanceRepository), new TestUnitOfWork());

        await service.EditItem(EditDto(1,
            new ShipmentItemEditDto { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 6 }));

        Assert.Equal(10, await BalanceQuantity(balanceRepository));
    }

    [Fact]
    public async Task EditItem_ApprovedShipment_NotEnoughResources_Throws()
    {
        var shipmentRepository = new TestShipmentRepository();
        await shipmentRepository.EditItem(Shipment(1, true,
            new ShipmentItemEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 4 }));

        var balanceRepository = new TestBalanceRepository();
        await balanceRepository.EditItem(new BalanceEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 10 });

        var service = new ShipmentService(shipmentRepository, new BalanceService(balanceRepository), new TestUnitOfWork());

        //10 + 4 - 20 < 0 — остатка не хватает
        await Assert.ThrowsAsync<UserException>(() => service.EditItem(EditDto(1,
            new ShipmentItemEditDto { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 20 })));
    }

    [Fact]
    public async Task DeleteItem_ApprovedShipment_ReturnsBalance()
    {
        var shipmentRepository = new TestShipmentRepository();
        await shipmentRepository.EditItem(Shipment(1, true,
            new ShipmentItemEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 4 }));

        var balanceRepository = new TestBalanceRepository();
        await balanceRepository.EditItem(new BalanceEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 10 });

        var service = new ShipmentService(shipmentRepository, new BalanceService(balanceRepository), new TestUnitOfWork());

        await service.DeleteItem(1);

        Assert.Equal(14, await BalanceQuantity(balanceRepository));
        var shipments = await shipmentRepository.GetAll(null);
        Assert.Empty(shipments.Items);
    }

    [Fact]
    public async Task DeleteItem_UnapprovedShipment_BalanceUntouched()
    {
        var shipmentRepository = new TestShipmentRepository();
        await shipmentRepository.EditItem(Shipment(1, false,
            new ShipmentItemEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 4 }));

        var balanceRepository = new TestBalanceRepository();
        await balanceRepository.EditItem(new BalanceEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 10 });

        var service = new ShipmentService(shipmentRepository, new BalanceService(balanceRepository), new TestUnitOfWork());

        await service.DeleteItem(1);

        Assert.Equal(10, await BalanceQuantity(balanceRepository));
        var shipments = await shipmentRepository.GetAll(null);
        Assert.Empty(shipments.Items);
    }
}
