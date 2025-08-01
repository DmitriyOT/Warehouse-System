using Warehouse.Contracts.Api.Request;
using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Application;

/// <summary>
/// Интерфейс для создания, чтения, изменения, удаления объектов
/// </summary>
public interface ICrudService<TEntity> where TEntity : BaseEntityWithId
{
    /// <summary>
    /// Получить список для грида
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public Task<Tuple<List<TEntity>, long>> GetAll(GridOptionsDto options);

    /// <summary>
    /// Получить один элемент для карточки
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Сущность</returns>
    public Task<TEntity> GetItem(long id);

    /// <summary>
    /// Изменить или создать элемент
    /// </summary>
    /// <param name="item"></param>
    /// <returns>id объекта</returns>
    public Task<long> EditItem(TEntity item);

    /// <summary>
    /// Удалить список элементов
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Если нет exception, то удалился корректно, 
    /// ничего не возвращает</returns>
    public Task DeleteItems(IEnumerable<long> id);

    /// <summary>
    /// Удалить элемент
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Если нет exception, то удалился корректно, 
    /// ничего не возвращает</returns>
    public Task DeleteItem(long id);
}
