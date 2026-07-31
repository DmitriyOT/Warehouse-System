using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Controllers.Base;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Баланс. Остатки меняются только документами (поступление/отгрузка),
/// поэтому контроллер поддерживает только операции чтения
/// </summary>
[ApiController]
[Route("[controller]")]
public class BalanceController : BaseReadController<BalanceEntity>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public BalanceController(ILogger<BalanceController> logger, IBalanceService crudService)
        : base(logger, crudService)
    {
    }
}