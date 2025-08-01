using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UnitController : ControllerBase
{

    private readonly ILogger<UnitController> _logger;

    public UnitController(ILogger<UnitController> logger)
    {
        _logger = logger;
    }
}
