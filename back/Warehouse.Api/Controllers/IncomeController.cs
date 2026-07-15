using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Controllers.Base;
using Warehouse.Application.Services;
using Warehouse.Contracts.Api.Request.Dtos;
using Warehouse.Contracts.Api.Response;
using Warehouse.Domain.Models;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Поступления контроллер
/// </summary>
[ApiController]
[Route("[controller]")]
public class IncomeController : BaseCrudController<IncomeEntity>
{
    private readonly IncomeService _incomeService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public IncomeController(ILogger<IncomeController> logger, IncomeService crudService)
        : base(logger, crudService)
    {
        _incomeService = crudService;
    }

    /// <summary>
    /// Создать или отредактировать документ поступления
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("Edit")]
    public async Task<ActionResult> Edit(IncomeEditDto dto)
    {
        var result = await _incomeService.EditItem(dto);
        return Ok(new ResponseDto<long>(result));
    }

    /// <summary>
    /// Устаревший endpoint создания/редактирования на основе сущности отключён
    /// </summary>
    [NonAction]
    public override Task<ActionResult> EditItem(IncomeEntity entity)
    {
        throw new NotSupportedException("Use POST /Income/Edit instead.");
    }
}
