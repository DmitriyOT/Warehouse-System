using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Contracts.Api.Request;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Contracts.Infrastracture;

/// <summary>
/// Интерфейс для обощённого CRUD репозитория
/// </summary>
/// <typeparam name="Entity"></typeparam>
public interface ICrudRepository<Entity> where Entity : BaseEntityWithId
{
    /// <summary>
    /// Получить список для грида
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public Task<Tuple<List<Entity>, long>> GetAll(GridOptionsDto options);

    /// <summary>
    /// Получить один элемент для карточки
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Сущность</returns>
    public Task<Entity> GetItem(long id);

    /// <summary>
    /// Изменить или создать элемент
    /// </summary>
    /// <param name="item"></param>
    /// <returns>id объекта</returns>
    public Task<long> EditItem(Entity item);

    /// <summary>
    /// Удалить список элементов
    /// </summary>
    /// <param name="ids"></param>
    /// <returns>Если нет exception, то удалился корректно, 
    /// ничего не возвращает</returns>
    public Task DeleteItems(IEnumerable<long> ids);

    /// <summary>
    /// Удалить элемент
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Если нет exception, то удалился корректно, ничего не возвращает</returns>
    public Task DeleteItem(long id);
}
