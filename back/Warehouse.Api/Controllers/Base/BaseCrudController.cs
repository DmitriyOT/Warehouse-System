using Microsoft.AspNetCore.Mvc;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Api.Controllers.Base;

/// <summary>
/// Базовый класс контроллера для реализации CRUD операций в нём
/// </summary>
/// <typeparam name="Entity"></typeparam>
public abstract class BaseCrudController<Entity> : BaseReadController<Entity> where Entity : BaseEntityWithId
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public BaseCrudController(ILogger<BaseCrudController<Entity>> logger, ICrudService<Entity> crudService)
        : base(logger, crudService)
    {
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
    public virtual async Task<ActionResult> EditItem(Entity entity)
    {
        var result = await _crudService.EditItem(entity);
        return Ok(new ResponseDto<long>(result));
    }
}
