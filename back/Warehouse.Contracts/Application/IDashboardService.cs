using Warehouse.Contracts.Api.Response.Dtos;

namespace Warehouse.Contracts.Application;

/// <summary>
/// Сервис агрегации данных для дашборда склада
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Получить сводные данные дашборда
    /// </summary>
    /// <returns></returns>
    public Task<DashboardSummaryDto> GetSummary();
}
