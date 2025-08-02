using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Contracts.Api.Request;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Contracts.Infrastracture;

/// <summary>
/// Интерфейс для обощённого CRUD репозитория с дополнительным методов архива
/// </summary>
/// <typeparam name="Entity"></typeparam>
public interface IArchiveCrudRepository<Entity> : ICrudRepository<Entity> where Entity : BaseEntityWithIdArchive
{
    /// <summary>
    /// Изменить статус архива у объекта
    /// </summary>
    /// <param name="id">Id объекта</param>
    /// <param name="newState">Новый статус</param>
    /// <returns>ничего</returns>
    public Task SetArchiveItemState(long id, bool newState);
}
