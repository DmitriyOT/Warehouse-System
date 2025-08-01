using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{

    private readonly ILogger<ClientController> _logger;

    public ClientController(ILogger<ClientController> logger)
    {
        _logger = logger;
    }
}
