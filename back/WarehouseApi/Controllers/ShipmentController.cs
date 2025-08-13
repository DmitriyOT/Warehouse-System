using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Controllers.Base;
using Warehouse.Application.Services;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Отгрузки
/// </summary>
[ApiController]
[Route("[controller]")]
public class ShipmentController : BaseCrudController<ShipmentEntity>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public ShipmentController(ILogger<ShipmentController> logger, ShipmentService crudService)
        : base(logger, crudService)
    {
    }
}
