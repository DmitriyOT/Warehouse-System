using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Controllers.Base;
using Warehouse.Application.Services;
using Warehouse.Contracts.Api.Request.Dtos;
using Warehouse.Contracts.Api.Response;
using Warehouse.Domain.Models;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Отгрузки
/// </summary>
[ApiController]
[Route("[controller]")]
public class ShipmentController : BaseCrudController<ShipmentEntity>
{
    private readonly ShipmentService _shipmentService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public ShipmentController(ILogger<ShipmentController> logger, ShipmentService crudService)
        : base(logger, crudService)
    {
        _shipmentService = crudService;
    }

    /// <summary>
    /// Создать или отредактировать документ отгрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("Edit")]
    public async Task<ActionResult> Edit(ShipmentEditDto dto)
    {
        var result = await _shipmentService.EditItem(dto);
        return Ok(new ResponseDto<long>(result));
    }

    /// <summary>
    /// Изменить состояние на подписана или не подписана
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newStateCode">approve или disApprove</param>
    /// <returns></returns>
    [HttpPut("ChangeState")]
    public async Task<ActionResult> ChangeState(long id, string newStateCode)
    {
        await _shipmentService.ChangeStateAsync(id, newStateCode);
        return Ok(new ResponseDtoEmpty());
    }

    /// <summary>
    /// Устаревший endpoint создания/редактирования на основе сущности отключён
    /// </summary>
    [NonAction]
    public override Task<ActionResult> EditItem(ShipmentEntity entity)
    {
        throw new NotSupportedException("Use POST /Shipment/Edit instead.");
    }
}
