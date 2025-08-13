using System.Threading.Tasks;
using Warehouse.Contracts.Application;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Application.Services.Base;

/// <summary>
/// CRUD сервис плюс поддержа состояния архива
/// </summary>
/// <typeparam name="Entity"></typeparam>
public class ArchiveCrudService<Entity> : CrudService<Entity>, IArchiveCrudService<Entity> where Entity : BaseEntityWithIdArchive
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="repository"></param>
    public ArchiveCrudService(IArchiveCrudRepository<Entity> repository) : base(repository)
    {
    }

    /// <summary>
    /// Установить новое состояние архива
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newState"></param>
    /// <returns></returns>
    public async Task SetArchiveItemState(long id, bool newState)
    {
        var archiveRepository = _repository as IArchiveCrudRepository<Entity>;
        if(archiveRepository != null)
        {
            await archiveRepository.SetArchiveItemState(id, newState);
        }
    }
}
