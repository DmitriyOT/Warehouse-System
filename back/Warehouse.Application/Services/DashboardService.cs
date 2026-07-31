using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Warehouse.Contracts.Api.Response.Dtos;
using Warehouse.Contracts.Application;
using Warehouse.Contracts.Infrastructure;

namespace Warehouse.Application.Services;

/// <summary>
/// Сервис агрегации данных для дашборда склада.
/// Неделя считается как скользящее окно из последних 7 дней, включая сегодня (UTC),
/// предыдущая неделя — 7 дней до него. Даты документов — DateOnly, сегодня —
/// DateOnly.FromDateTime(DateTime.UtcNow).
/// Запросы выполняются асинхронно (async-расширения EF Core поверх IQueryable
/// репозитория): агрегация (GroupBy/Sum/Count) транслируется в SQL.
/// Результат кэшируется в IMemoryCache на <see cref="CacheTtl"/> — сводка
/// допускает небольшое отставание, зато не делаем 10+ запросов к БД на каждый GET.
/// </summary>
public class DashboardService : IDashboardService
{
    private const int WeekDays = 7;
    private const int LastOperationsCount = 5;

    /// <summary>
    /// Ключ кэша сводки дашборда (у сводки нет параметров, ключ один)
    /// </summary>
    private const string SummaryCacheKey = "DashboardSummary";

    /// <summary>
    /// Время жизни сводки в кэше
    /// </summary>
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(30);

    private readonly IDashboardRepository _dashboardRepository;
    private readonly IMemoryCache _memoryCache;

    public DashboardService(IDashboardRepository dashboardRepository, IMemoryCache memoryCache)
    {
        _dashboardRepository = dashboardRepository;
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Получить сводные данные дашборда
    /// </summary>
    /// <returns></returns>
    public async Task<DashboardSummaryDto> GetSummary()
    {
        if (_memoryCache.TryGetValue(SummaryCacheKey, out DashboardSummaryDto? cached) && cached != null)
        {
            return cached;
        }

        var result = await BuildSummary();
        _memoryCache.Set(SummaryCacheKey, result, CacheTtl);
        return result;
    }

    /// <summary>
    /// Собрать сводку запросами к БД
    /// </summary>
    private async Task<DashboardSummaryDto> BuildSummary()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var weekStart = today.AddDays(-(WeekDays - 1));
        var prevWeekStart = weekStart.AddDays(-WeekDays);

        //Остаток по неархивным ресурсам и единицам измерения
        var totalBalance = await _dashboardRepository.Balances
            .Where(x => x.Resource != null && !x.Resource.IsArchive
                     && x.Unit != null && !x.Unit.IsArchive)
            .SumAsync(x => x.Quantity);

        //Приходы и подтверждённые отгрузки по дням за текущую неделю
        var incomeByDay = await _dashboardRepository.IncomeItems
            .Where(x => x.Income != null && x.Income.Date >= weekStart && x.Income.Date <= today)
            .GroupBy(x => x.Income!.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Quantity) })
            .ToListAsync();

        var shipmentByDay = await _dashboardRepository.ShipmentItems
            .Where(x => x.Shipment != null && x.Shipment.IsApprove
                     && x.Shipment.Date >= weekStart && x.Shipment.Date <= today)
            .GroupBy(x => x.Shipment!.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Quantity) })
            .ToListAsync();

        var weekIncomeTotal = incomeByDay.Sum(x => x.Total);
        var weekShipmentTotal = shipmentByDay.Sum(x => x.Total);

        //Движение за неделю с заполнением нулями пустых дней
        var weekMovement = new List<DashboardDayDto>();
        for (var i = 0; i < WeekDays; i++)
        {
            var date = weekStart.AddDays(i);
            weekMovement.Add(new DashboardDayDto
            {
                Date = date,
                Income = incomeByDay.FirstOrDefault(x => x.Date == date)?.Total ?? 0,
                Shipment = shipmentByDay.FirstOrDefault(x => x.Date == date)?.Total ?? 0
            });
        }

        //Приближённая дельта остатка: истории баланса нет, поэтому
        //остаток неделю назад = текущий − приходы недели + подтверждённые отгрузки недели
        decimal balanceDeltaPercent = 0;
        var weekAgoBalance = totalBalance - weekIncomeTotal + weekShipmentTotal;
        if (weekAgoBalance != 0)
        {
            balanceDeltaPercent = (decimal)(totalBalance - weekAgoBalance) / weekAgoBalance * 100;
        }

        //Количество документов за текущую и предыдущую неделю
        var incomeCount = await _dashboardRepository.Incomes
            .CountAsync(x => x.Date >= weekStart && x.Date <= today);
        var incomeCountPrev = await _dashboardRepository.Incomes
            .CountAsync(x => x.Date >= prevWeekStart && x.Date < weekStart);
        var shipmentCount = await _dashboardRepository.Shipments
            .CountAsync(x => x.Date >= weekStart && x.Date <= today);
        var shipmentCountPrev = await _dashboardRepository.Shipments
            .CountAsync(x => x.Date >= prevWeekStart && x.Date < weekStart);

        var activeClientCount = await _dashboardRepository.Clients.CountAsync(x => !x.IsArchive);

        //Последние операции: приходы с плюсом, подтверждённые отгрузки с минусом
        var incomeOperations = _dashboardRepository.IncomeItems
            .Where(x => x.Income != null && x.Resource != null)
            .Select(x => new { Date = x.Income!.Date, ResourceName = x.Resource!.Name, Quantity = x.Quantity });
        var shipmentOperations = _dashboardRepository.ShipmentItems
            .Where(x => x.Shipment != null && x.Shipment.IsApprove && x.Resource != null)
            .Select(x => new { Date = x.Shipment!.Date, ResourceName = x.Resource!.Name, Quantity = -x.Quantity });

        var lastOperations = (await incomeOperations.Union(shipmentOperations)
            .OrderByDescending(x => x.Date)
            .Take(LastOperationsCount)
            .ToListAsync())
            .Select(x => new DashboardOperationDto { ResourceName = x.ResourceName, Quantity = x.Quantity })
            .ToList();

        var result = new DashboardSummaryDto
        {
            Kpis = new DashboardKpisDto
            {
                TotalBalance = totalBalance,
                BalanceDeltaPercent = balanceDeltaPercent,
                IncomeCount = incomeCount,
                IncomeDelta = incomeCount - incomeCountPrev,
                ShipmentCount = shipmentCount,
                ShipmentDelta = shipmentCount - shipmentCountPrev,
                ActiveClientCount = activeClientCount,
                //У справочников нет даты создания, настоящую дельту посчитать невозможно
                ClientDelta = 0
            },
            WeekMovement = weekMovement,
            LastOperations = lastOperations
        };

        return result;
    }
}
