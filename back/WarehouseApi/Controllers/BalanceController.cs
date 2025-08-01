using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BalanceController : ControllerBase
{

    private readonly ILogger<BalanceController> _logger;

    public BalanceController(ILogger<BalanceController> logger)
    {
        _logger = logger;
    }
}
