using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Api.Controllers.Base;

/// <summary>
/// Базовый класс контроллера только для операций чтения данных
/// </summary>
/// <typeparam name="Entity"></typeparam>
[Authorize]
public abstract class BaseReadController<Entity> : ControllerBase where Entity : BaseEntityWithId
{
    /// <summary>
    /// Логирование
    /// </summary>
    protected ILogger<BaseReadController<Entity>> _logger { get; }
    /// <summary>
    /// Сервис с поддержкой CRUD операций над данными
    /// </summary>
    protected ICrudService<Entity> _crudService { get; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public BaseReadController(ILogger<BaseReadController<Entity>> logger, ICrudService<Entity> crudService)
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
        var page = new PageView(options.Page, options.PageSize, result.TotalCount);
        return Ok(new ResponseDtoGrid<Entity>(result.Items, page));
    }
}
