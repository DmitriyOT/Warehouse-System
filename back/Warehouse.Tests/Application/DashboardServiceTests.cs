using Microsoft.Extensions.Caching.Memory;
using Warehouse.Application.Services;
using Warehouse.Domain.Models;
using Warehouse.Tests.TestInfrastructure;

namespace Warehouse.Tests.Application;

public class DashboardServiceTests
{
    private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

    private static ResourceEntity Resource(long id, bool isArchive = false, string name = "Ресурс")
        => new ResourceEntity { Id = id, Name = name, IsArchive = isArchive };

    private static UnitEntity Unit(long id, bool isArchive = false)
        => new UnitEntity { Id = id, Name = "шт", IsArchive = isArchive };

    private static IncomeEntity Income(long id, DateOnly date, params IncomeItemEntity[] items)
    {
        var income = new IncomeEntity { Id = id, Number = $"П{id}", Date = date, IncomeItems = items };
        foreach (var item in items)
            item.Income = income;
        return income;
    }

    private static ShipmentEntity Shipment(long id, DateOnly date, bool isApprove, params ShipmentItemEntity[] items)
    {
        var shipment = new ShipmentEntity
        {
            Id = id,
            Number = $"О{id}",
            Date = date,
            ClientId = 1,
            IsApprove = isApprove,
            ShipmentItems = items
        };
        foreach (var item in items)
            item.Shipment = shipment;
        return shipment;
    }

    [Fact]
    public async Task GetSummary_TotalBalance_OnlyNonArchivedResourcesAndUnits()
    {
        var repository = new TestDashboardRepository();
        repository.BalanceList.Add(new BalanceEntity { Id = 1, Quantity = 10, Resource = Resource(1), Unit = Unit(1) });
        repository.BalanceList.Add(new BalanceEntity { Id = 2, Quantity = 20, Resource = Resource(2), Unit = Unit(2) });
        repository.BalanceList.Add(new BalanceEntity { Id = 3, Quantity = 100, Resource = Resource(3, isArchive: true), Unit = Unit(3) });
        repository.BalanceList.Add(new BalanceEntity { Id = 4, Quantity = 100, Resource = Resource(4), Unit = Unit(4, isArchive: true) });

        var service = new DashboardService(repository, new MemoryCache(new MemoryCacheOptions()));

        var summary = await service.GetSummary();

        Assert.Equal(30, summary.Kpis.TotalBalance);
    }

    [Fact]
    public async Task GetSummary_WeekMovement_ZeroFillsEmptyDays()
    {
        var repository = new TestDashboardRepository();
        var resource = Resource(1);
        repository.IncomeList.Add(Income(1, Today, new IncomeItemEntity { Id = 1, Quantity = 5, Resource = resource }));
        repository.IncomeList.Add(Income(2, Today.AddDays(-6), new IncomeItemEntity { Id = 2, Quantity = 10, Resource = resource }));
        //Вне недели — не должно попасть в график
        repository.IncomeList.Add(Income(3, Today.AddDays(-7), new IncomeItemEntity { Id = 3, Quantity = 100, Resource = resource }));
        repository.ShipmentList.Add(Shipment(1, Today, true, new ShipmentItemEntity { Id = 1, Quantity = 3, Resource = resource }));

        var service = new DashboardService(repository, new MemoryCache(new MemoryCacheOptions()));

        var summary = await service.GetSummary();

        Assert.Equal(7, summary.WeekMovement.Count);
        Assert.Equal(Today.AddDays(-6), summary.WeekMovement[0].Date);
        Assert.Equal(Today, summary.WeekMovement[6].Date);

        Assert.Equal(10, summary.WeekMovement[0].Income);
        Assert.Equal(0, summary.WeekMovement[1].Income);
        Assert.Equal(5, summary.WeekMovement[6].Income);
        Assert.Equal(3, summary.WeekMovement[6].Shipment);
        Assert.Equal(0, summary.WeekMovement[5].Shipment);
    }

    [Fact]
    public async Task GetSummary_WeekMovement_UnapprovedShipmentsNotCounted()
    {
        var repository = new TestDashboardRepository();
        var resource = Resource(1);
        repository.ShipmentList.Add(Shipment(1, Today, false, new ShipmentItemEntity { Id = 1, Quantity = 7, Resource = resource }));

        var service = new DashboardService(repository, new MemoryCache(new MemoryCacheOptions()));

        var summary = await service.GetSummary();

        Assert.Equal(0, summary.WeekMovement[6].Shipment);
        //Но в количество документов отгрузок попадают все статусы
        Assert.Equal(1, summary.Kpis.ShipmentCount);
    }

    [Fact]
    public async Task GetSummary_Kpis_DeltasVsPreviousWeek()
    {
        var repository = new TestDashboardRepository();
        repository.IncomeList.Add(Income(1, Today));
        repository.IncomeList.Add(Income(2, Today.AddDays(-2)));
        repository.IncomeList.Add(Income(3, Today.AddDays(-8)));
        repository.ShipmentList.Add(Shipment(1, Today.AddDays(-1), true));
        repository.ShipmentList.Add(Shipment(2, Today.AddDays(-9), true));
        repository.ShipmentList.Add(Shipment(3, Today.AddDays(-10), true));

        var service = new DashboardService(repository, new MemoryCache(new MemoryCacheOptions()));

        var summary = await service.GetSummary();

        Assert.Equal(2, summary.Kpis.IncomeCount);
        Assert.Equal(1, summary.Kpis.IncomeDelta);
        Assert.Equal(1, summary.Kpis.ShipmentCount);
        Assert.Equal(-1, summary.Kpis.ShipmentDelta);
    }

    [Fact]
    public async Task GetSummary_BalanceDeltaPercent_ByWeekMovement()
    {
        //Остаток 100, приход за неделю 50, подтверждённые отгрузки 30
        //Остаток неделю назад = 100 - 50 + 30 = 80, дельта = (100 - 80) / 80 * 100 = 25%
        var repository = new TestDashboardRepository();
        var resource = Resource(1);
        repository.BalanceList.Add(new BalanceEntity { Id = 1, Quantity = 100, Resource = resource, Unit = Unit(1) });
        repository.IncomeList.Add(Income(1, Today, new IncomeItemEntity { Id = 1, Quantity = 50, Resource = resource }));
        repository.ShipmentList.Add(Shipment(1, Today, true, new ShipmentItemEntity { Id = 1, Quantity = 30, Resource = resource }));

        var service = new DashboardService(repository, new MemoryCache(new MemoryCacheOptions()));

        var summary = await service.GetSummary();

        Assert.Equal(25, summary.Kpis.BalanceDeltaPercent);
    }

    [Fact]
    public async Task GetSummary_BalanceDeltaPercent_ZeroWhenWeekAgoBalanceIsZero()
    {
        var repository = new TestDashboardRepository();
        var resource = Resource(1);
        repository.BalanceList.Add(new BalanceEntity { Id = 1, Quantity = 10, Resource = resource, Unit = Unit(1) });
        repository.IncomeList.Add(Income(1, Today, new IncomeItemEntity { Id = 1, Quantity = 10, Resource = resource }));

        var service = new DashboardService(repository, new MemoryCache(new MemoryCacheOptions()));

        var summary = await service.GetSummary();

        Assert.Equal(0, summary.Kpis.BalanceDeltaPercent);
    }

    [Fact]
    public async Task GetSummary_ActiveClientCount_OnlyNonArchived()
    {
        var repository = new TestDashboardRepository();
        repository.ClientList.Add(new ClientEntity { Id = 1, Name = "Клиент 1", Address = "Адрес 1", IsArchive = false });
        repository.ClientList.Add(new ClientEntity { Id = 2, Name = "Клиент 2", Address = "Адрес 2", IsArchive = false });
        repository.ClientList.Add(new ClientEntity { Id = 3, Name = "Клиент 3", Address = "Адрес 3", IsArchive = true });

        var service = new DashboardService(repository, new MemoryCache(new MemoryCacheOptions()));

        var summary = await service.GetSummary();

        Assert.Equal(2, summary.Kpis.ActiveClientCount);
        Assert.Equal(0, summary.Kpis.ClientDelta);
    }

    [Fact]
    public async Task GetSummary_LastOperations_SignsAndOrder()
    {
        var repository = new TestDashboardRepository();
        repository.IncomeList.Add(Income(1, Today.AddDays(-1),
            new IncomeItemEntity { Id = 1, Quantity = 100, Resource = Resource(1, name: "Сталь") }));
        repository.ShipmentList.Add(Shipment(1, Today, true,
            new ShipmentItemEntity { Id = 1, Quantity = 30, Resource = Resource(2, name: "Медь") }));
        //Неподтверждённая отгрузка не должна попасть в операции
        repository.ShipmentList.Add(Shipment(2, Today, false,
            new ShipmentItemEntity { Id = 2, Quantity = 999, Resource = Resource(3, name: "Цинк") }));

        var service = new DashboardService(repository, new MemoryCache(new MemoryCacheOptions()));

        var summary = await service.GetSummary();

        Assert.Equal(2, summary.LastOperations.Count);
        //Сортировка по дате документа по убыванию
        Assert.Equal("Медь", summary.LastOperations[0].ResourceName);
        Assert.Equal(-30, summary.LastOperations[0].Quantity);
        Assert.Equal("Сталь", summary.LastOperations[1].ResourceName);
        Assert.Equal(100, summary.LastOperations[1].Quantity);
    }

    [Fact]
    public async Task GetSummary_LastOperations_TakesOnlyFive()
    {
        var repository = new TestDashboardRepository();
        var resource = Resource(1, name: "Сталь");
        for (var i = 1; i <= 7; i++)
        {
            repository.IncomeList.Add(Income(i, Today.AddDays(-i % 7),
                new IncomeItemEntity { Id = i, Quantity = i, Resource = resource }));
        }

        var service = new DashboardService(repository, new MemoryCache(new MemoryCacheOptions()));

        var summary = await service.GetSummary();

        Assert.Equal(5, summary.LastOperations.Count);
    }
}
