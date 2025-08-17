using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Api.Controllers.Interfaces;

public interface ICrudController
{
    public Task<ActionResult> GetItem(long id);
}
