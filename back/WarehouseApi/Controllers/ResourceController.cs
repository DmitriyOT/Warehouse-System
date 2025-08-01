using Microsoft.AspNetCore.Mvc;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourceController : BaseCrudController<ResourceEntity>
{

    public ResourceController(ILogger<ResourceController> logger, ICrudService<ResourceEntity> crudService)
        : base(logger, crudService)
    {
    }

    [HttpPost("testError")]
    public ActionResult TestError()
    {
        throw new NotImplementedException();
    }
}
