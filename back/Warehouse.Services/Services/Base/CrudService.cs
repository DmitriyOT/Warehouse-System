using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Application;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Application.Services.Base;

/// <summary>
/// Универсальный сервис для CRUD операций с объектами
/// </summary>
/// <typeparam name="Entity"></typeparam>
public class CrudService<Entity> : ICrudService<Entity> where Entity : BaseEntityWithId
{
    /// <summary>
    /// Репозиторий
    /// </summary>
    protected ICrudRepository<Entity> _repository { get; set; }
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="repository"></param>
    public CrudService(ICrudRepository<Entity> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Удалить элементы
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task DeleteItems(IEnumerable<long> ids)
    {
        await _repository.DeleteItems(ids);
    }

    /// <summary>
    /// Удалить элемент
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteItem(long id)
    {
        await _repository.DeleteItem(id);
    }

    /// <summary>
    /// Отредактировать или создать элемент
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<long> EditItem(Entity item)
    {
        return await _repository.EditItem(item);
    }

    /// <summary>
    /// Грид данных
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<Tuple<List<Entity>, long>> GetAll(GridOptionsDto options)
    {
        return await _repository.GetAll(options);
    }

    /// <summary>
    /// Одиночный элемент
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Entity> GetItem(long id)
    {
        return await _repository.GetItem(id);
    }
}

