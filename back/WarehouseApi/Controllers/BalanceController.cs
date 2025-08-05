using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Баланс
/// </summary>
[ApiController]
[Route("[controller]")]
public class BalanceController : ControllerBase
{

    private readonly ILogger<BalanceController> _logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    public BalanceController(ILogger<BalanceController> logger)
    {
        _logger = logger;
    }
}
