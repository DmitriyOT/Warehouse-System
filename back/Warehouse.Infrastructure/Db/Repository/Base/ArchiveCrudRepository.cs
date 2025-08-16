using Microsoft.EntityFrameworkCore;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Contracts.Exceptions;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Infrastructure.Db.Repository.Base;

/// <summary>
/// Репозиторий для удобной работы с объектами, поддерживающими архив
/// </summary>
/// <typeparam name="Entity"></typeparam>
public class ArchiveCrudRepository<Entity> : CrudRepository<Entity>, IArchiveCrudRepository<Entity> where Entity : BaseEntityWithIdArchiveName
{
    /// <summary>
    /// Конструктор репозитория
    /// </summary>
    /// <param name="db"></param>
    public ArchiveCrudRepository(PostgresDbContext db) : base(db)
    {
    }

    /// <summary>
    /// Установить статус архива у объекта
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Ничего</returns>
    public async Task SetArchiveItemState(long id, bool newState)
    {
        var itemDb = await entities
            .FirstOrDefaultAsync(x => x.Id == id);

        if (itemDb == null)
        {
            throw new UserException("Ошибка. Объект не найден.");
        }

        itemDb.IsArchive = newState;
        await DB.SaveChangesAsync();
    }

    protected override async Task PreCreateEditCheck(Entity item, Entity? oldItem)
    {
        var existEntity = await entities.Where(x => x.Name == item.Name).AsNoTracking().FirstOrDefaultAsync();
        if(existEntity != null && existEntity.Id != item.Id)
        {
            throw new UserException($"Ошибка создания. Объект данного типа с наименованием <{item.Name}> уже есть в системе.");
        }
    }
}
