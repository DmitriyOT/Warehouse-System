using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class IncomeController : ControllerBase
{

    private readonly ILogger<IncomeController> _logger;

    public IncomeController(ILogger<IncomeController> logger)
    {
        _logger = logger;
    }
}
