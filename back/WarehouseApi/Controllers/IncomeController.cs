using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Controllers.Base;
using Warehouse.Application.Services;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Поступления контроллер
/// </summary>
[ApiController]
[Route("[controller]")]
public class IncomeController : BaseCrudController<IncomeEntity>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public IncomeController(ILogger<IncomeController> logger, IncomeService crudService) 
        : base(logger, crudService)
    {
    }
}
