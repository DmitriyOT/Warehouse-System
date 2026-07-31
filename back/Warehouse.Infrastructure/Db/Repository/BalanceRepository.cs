using Microsoft.EntityFrameworkCore;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Infrastructure;
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
        return await entities.AsNoTracking().FirstOrDefaultAsync(x => x.ResourceId == resourceId && x.UnitId == unitId);
    }

    /// <summary>
    /// Атомарно применить изменение количества к строке баланса одним UPDATE
    /// </summary>
    public async Task<bool> TryApplyDeltaAsync(long resourceId, long unitId, long delta)
    {
        if (delta < 0)
        {
            //Остаток не должен уйти в минус — условие проверяется на стороне БД в том же UPDATE.
            //Строгое неравенство: нулевой остаток нельзя записать из-за check-констрейнта (Quantity > 0)
            var updated = await entities
                .Where(x => x.ResourceId == resourceId && x.UnitId == unitId && x.Quantity + delta > 0)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.Quantity, x => x.Quantity + delta));
            if (updated > 0)
            {
                return true;
            }

            //Списание всего остатка в ноль — строку удаляем, нулевые остатки не храним
            var deleted = await entities
                .Where(x => x.ResourceId == resourceId && x.UnitId == unitId && x.Quantity + delta == 0)
                .ExecuteDeleteAsync();
            return deleted > 0;
        }

        var rows = await entities
            .Where(x => x.ResourceId == resourceId && x.UnitId == unitId)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Quantity, x => x.Quantity + delta));
        return rows > 0;
    }

    /// <summary>
    /// Получить список элементов для грида
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public override async Task<PagedResult<BalanceEntity>> GetAll(GridOptionsDto options)
    {
        var query = GetQuery(options);
        var items = await query.OrderBy(x => x.Id)
            .Include(x => x.Unit).Include(x => x.Resource)
            .Skip(options.GetSkip()).Take(options.GetTake())//Paginations
            .AsNoTracking().ToListAsync();//To array (List)
        var count = await query.LongCountAsync();
        return new PagedResult<BalanceEntity>(items, count);
    }
}
