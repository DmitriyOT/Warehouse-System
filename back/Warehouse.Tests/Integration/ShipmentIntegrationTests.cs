using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Services;
using Warehouse.Contracts.Api.Request.Dtos;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Db;
using Warehouse.Infrastructure.Db.Repository;

namespace Warehouse.Tests.Integration;

/// <summary>
/// Интеграционный тест отгрузки на реальном PostgreSQL:
/// удаление подписанной отгрузки возвращает товар на баланс.
/// </summary>
[Collection(PostgresCollection.Name)]
public class ShipmentIntegrationTests
{
    private readonly PostgresFixture _fixture;

    public ShipmentIntegrationTests(PostgresFixture fixture)
    {
        _fixture = fixture;
    }

    [SkippableFact]
    public async Task DeleteItem_ApprovedShipment_ReturnsItemsToBalance()
    {
        Skip.IfNot(_fixture.IsAvailable, _fixture.UnavailableReason);

        await using var db = await IsolatedDatabase.CreateAsync(_fixture);
        await db.MigrateAsync();
        var (resourceId, unitId) = await db.SeedResourceAndUnitAsync();

        long clientId;
        await using (var context = db.CreateContext())
        {
            var client = new ClientEntity { Name = "Тестовый клиент", Address = "Тестовый адрес" };
            context.Clients.Add(client);
            context.Balances.Add(new BalanceEntity { ResourceId = resourceId, UnitId = unitId, Quantity = 10 });
            await context.SaveChangesAsync();
            clientId = client.Id;
        }

        // Сервис и репозитории поднимаются на реальном контексте, без test-double'ов.
        // Каждая операция — на своём контексте, как отдельные запросы к API
        // (в проде DbContext scoped на запрос): репозитории не всегда отсоединяют
        // загруженные сущности, и повторное использование контекста дало бы
        // конфликт отслеживания, которого в реальной работе нет.
        long shipmentId;
        await using (var context = db.CreateContext())
        {
            var service = CreateService(context);
            shipmentId = await service.EditItem(new ShipmentEditDto
            {
                Id = 0,
                Number = "S-1",
                Date = new DateOnly(2026, 7, 31),
                ClientId = clientId,
                ShipmentItems =
                [
                    new ShipmentItemEditDto { Id = 0, ResourceId = resourceId, UnitId = unitId, Quantity = 4 }
                ]
            });
        }

        // Подписание отгрузки списывает остаток: 10 - 4 = 6
        await using (var context = db.CreateContext())
        {
            var service = CreateService(context);
            await service.ChangeStateAsync(shipmentId, "approve");
        }

        // Удаление подписанной отгрузки должно вернуть товар на баланс: 6 + 4 = 10
        await using (var context = db.CreateContext())
        {
            var service = CreateService(context);
            await service.DeleteItem(shipmentId);
        }

        await using var checkContext = db.CreateContext();
        Assert.False(await checkContext.Shipments.AnyAsync());

        var balance = await checkContext.Balances
            .SingleAsync(x => x.ResourceId == resourceId && x.UnitId == unitId);
        Assert.Equal(10, balance.Quantity);
    }

    private static ShipmentService CreateService(PostgresDbContext context)
    {
        return new ShipmentService(
            new ShipmentRepository(context),
            new BalanceService(new BalanceRepository(context)),
            new EfUnitOfWork(context));
    }
}
