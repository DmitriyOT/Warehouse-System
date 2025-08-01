using Microsoft.AspNetCore.Mvc;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Контроллер для единиц измерения
/// </summary>
[ApiController]
[Route("[controller]")]
public class UnitController : BaseCrudController<UnitEntity>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public UnitController(ILogger<UnitController> logger, ICrudService<UnitEntity> crudService)
        : base(logger, crudService)
    {
    }
}
