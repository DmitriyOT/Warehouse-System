using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Api.Response.Dtos;
using Warehouse.Contracts.Application;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Дашборд: агрегированные показатели склада
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="dashboardService"></param>
    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Сводные данные для главной страницы: KPI, движение за неделю, последние операции
    /// </summary>
    /// <returns></returns>
    [HttpGet("summary")]
    public async Task<ActionResult> GetSummary()
    {
        var summary = await _dashboardService.GetSummary();
        return Ok(new ResponseDto<DashboardSummaryDto>(summary));
    }
}
