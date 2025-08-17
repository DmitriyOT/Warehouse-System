using Microsoft.AspNetCore.Mvc;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Application;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Api.Controllers.Base;

/// <summary>
/// Базовый класс контроллера для реализации надстройки над CRUD для архива
/// </summary>
/// <typeparam name="Entity"></typeparam>
public abstract class BaseArchiveCrudController<Entity> 
    : BaseCrudController<Entity> where Entity : BaseEntityWithIdArchiveName
{

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public BaseArchiveCrudController(
        ILogger<BaseArchiveCrudController<Entity>> logger, 
        IArchiveCrudService<Entity> crudService) 
        : base(logger, crudService)
    {
    }

    /// <summary>
    /// Изменить состояние архива объекта
    /// </summary>
    /// <param name="id">Id объекта</param>
    /// <param name="newState">Новое состояние</param>
    /// <returns>Ничего</returns>
    [HttpPut("EditArchiveItem")]
    public async Task<ActionResult> EditArchiveItem(long id, bool newState)
    {
        var archiveService = _crudService as IArchiveCrudService<Entity>;
        if (archiveService != null)
        {
            await archiveService.SetArchiveItemState(id, newState);
        }
        return Ok(new ResponseDtoEmpty());
    }
}
