using Warehouse.Domain.Models.Base;

namespace Warehouse.Contracts.Application;

/// <summary>
/// Сервис для работы с репозиторием архива и CRUD
/// </summary>
/// <typeparam name="Entity"></typeparam>
public interface IArchiveCrudService<Entity> : ICrudService<Entity> where Entity : BaseEntityWithIdArchive
{
    /// <summary>
    /// Изменить статус архива у объекта
    /// </summary>
    /// <param name="id">Id объекта</param>
    /// <param name="newState">Новый статус</param>
    /// <returns>ничего</returns>
    public Task SetArchiveItemState(long id, bool newState);
}
