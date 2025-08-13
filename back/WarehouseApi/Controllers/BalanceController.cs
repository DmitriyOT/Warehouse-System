using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Controllers.Base;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Баланс
/// </summary>
[ApiController]
[Route("[controller]")]
public class BalanceController : BaseCrudController<BalanceEntity>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public BalanceController(ILogger<BalanceController> logger, ICrudService<BalanceEntity> crudService)
        : base(logger, crudService)
    {
    }
}