using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Controllers.Interfaces;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Api.Controllers.Base;

/// <summary>
/// Базовый класс контроллера для реализации CRUD операций в нём
/// </summary>
/// <typeparam name="Entity"></typeparam>
public abstract class BaseCrudController<Entity> : ControllerBase, ICrudController where Entity : BaseEntityWithId
{
    protected ILogger<BaseCrudController<Entity>> _logger { get; }

    protected ICrudService<Entity> _crudService { get; }

    public BaseCrudController(ILogger<BaseCrudController<Entity>> logger, ICrudService<Entity> crudService)
    {
        _logger = logger;
        _crudService = crudService;
    }

    /// <summary>
    /// Получить один элемент по ID
    /// </summary>
    /// <returns></returns>
    [HttpGet("getItem")]
    public async Task<ActionResult> GetItem(long id)
    {
        var item = await _crudService.GetItem(id);
        return Ok(new ResponseDto<Entity>(item));
    }

    /// <summary>
    /// Получить данные для грида
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    [HttpPost("getAll")]
    public async Task<ActionResult> GetAll(GridOptionsDto options)
    {
        var result = await _crudService.GetAll(options);
        var response = result.Item1;
        var count = result.Item2;
        var page = new PageView(options.Page, options.PageSize, count);
        return Ok(new ResponseDtoGrid<Entity>(response, page));
    }

    /// <summary>
    /// Удалить элемент
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("DeleteItems")]
    public async Task<ActionResult> DeleteItems(long id)
    {
        await _crudService.DeleteItem(id);
        return Ok(new ResponseDtoEmpty());
    }

    /// <summary>
    /// Создать или отредактировать элемент
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost("EditItem")]
    public async Task<ActionResult> EditItem(Entity entity)
    {
        var result = await _crudService.EditItem(entity);
        return Ok(new ResponseDto<long>(result));
    }
}
