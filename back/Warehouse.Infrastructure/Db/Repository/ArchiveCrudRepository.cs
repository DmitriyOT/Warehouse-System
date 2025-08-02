using Microsoft.EntityFrameworkCore;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Infrastructure.Db.Repository;

/// <summary>
/// Репозиторий для удобной работы с объектами, поддерживающими архив
/// </summary>
/// <typeparam name="Entity"></typeparam>
public class ArchiveCrudRepository<Entity> : CrudRepository<Entity>, IArchiveCrudRepository<Entity> where Entity : BaseEntityWithIdArchive
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
            throw new InvalidOperationException("Error. Item not found.");
        }

        itemDb.IsArchive = newState;
        await DB.SaveChangesAsync();
    }
}
