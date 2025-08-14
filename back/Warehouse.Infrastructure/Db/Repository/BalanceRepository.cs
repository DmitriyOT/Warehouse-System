using Microsoft.EntityFrameworkCore;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Db.Repository.Base;

namespace Warehouse.Infrastructure.Db.Repository;

public class BalanceRepository : CrudRepository<BalanceEntity>, IBalanceRepository
{
    public BalanceRepository(PostgresDbContext db) : base(db)
    {
    }

    public async Task<BalanceEntity?> GetBalanceAsync(long resourceId, long unitId)
    {
        return await entities.FirstOrDefaultAsync(x => x.ResourceId == resourceId && x.UnitId == unitId);
    }

    /// <summary>
    /// Получить список элементов для грида
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public override async Task<Tuple<List<BalanceEntity>, long>> GetAll(GridOptionsDto options)
    {
        var query = GetQuery(options);
        return Tuple.Create(
            await query.OrderBy(x => x.Id)
                .Include(x => x.Unit).Include(x => x.Resource)
                .Skip(options.GetSkip()).Take(options.GetTake())//Paginations
                .AsNoTracking().ToListAsync(),//To array (List)
            await query.LongCountAsync()
            );
    }
}
