using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ShipmentController : ControllerBase
{

    private readonly ILogger<ShipmentController> _logger;

    public ShipmentController(ILogger<ShipmentController> logger)
    {
        _logger = logger;
    }
}
