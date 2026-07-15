using Microsoft.EntityFrameworkCore;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Db.Repository.Base;

namespace Warehouse.Infrastructure.Db.Repository;

public class IncomeRepository : CrudRepository<IncomeEntity>, IIncomeRepository
{
    public IncomeRepository(PostgresDbContext db) : base(db)
    {
    }

    /// <summary>
    /// Получить элемент без отслеживания изменений
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public override async Task<IncomeEntity> GetItem(long id)
    {
        var item = await entities.AsNoTracking()
            .Include(x => x.IncomeItems).ThenInclude(x => x.Resource)
            .Include(x => x.IncomeItems).ThenInclude(x => x.Unit)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (item == null)
        {
            throw new UserException($"Ошибка. Объект не найден в базе данных.");
        }
        else
        {
            return item;
        }
    }

    public override async Task<PagedResult<IncomeEntity>> GetAll(GridOptionsDto options)
    {
        var query = GetQuery(options);
        var items = await query.OrderBy(x => x.Id)
            .Include(x => x.IncomeItems).ThenInclude(x => x.Resource)
            .Include(x => x.IncomeItems).ThenInclude(x => x.Unit)
            .Skip(options.GetSkip()).Take(options.GetTake())//Paginations
            .AsNoTracking().ToListAsync();//To array (List)
        var count = await query.LongCountAsync();
        return new PagedResult<IncomeEntity>(items, count);
    }

    public override async Task<long> EditItem(IncomeEntity item)
    {
        var numberEqual = await entities.Where(x => x.Number == item.Number).AsNoTracking().FirstOrDefaultAsync();
        if (numberEqual != null && numberEqual.Id != item.Id)
        {
            throw new UserException($"Ошибка. Документ поступления с номером <{item.Number}> уже есть в системе.");
        }

        if (item.IncomeItems != null)
        {
            foreach (var incomeI in item.IncomeItems)
            {
                incomeI.Resource = null;
                incomeI.Unit = null;
            }
        }

        var itemDb = await entities
            .Include(x => x.IncomeItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        if (itemDb == null)
        {//Create New
            if (item.Id < 0)//Отрицательные id не используем, 0 тогда создастся корректный id
                item.Id = 0;

            entities.Add(item);

            if (item.IncomeItems != null)
            {
                foreach (var incomeItem in item.IncomeItems)
                {
                    incomeItem.Id = 0;
                    incomeItem.Income = item;
                }
                DB.IncomeItems.AddRange(item.IncomeItems);
            }
        }
        else
        {//edit exist
            entities.Attach(item);
            DB.Entry(item).State = EntityState.Modified;
            if (item.IncomeItems != null)
            {
                HashSet<long> itemsMap = new HashSet<long>();
                foreach (var incomeItem in item.IncomeItems)
                {
                    incomeItem.Income = item;
                    if (incomeItem.Id < 0)
                    {//add new items
                        incomeItem.Id = 0;
                        DB.IncomeItems.Add(incomeItem);
                    }
                    else
                    {//add to set exist items
                        itemsMap.Add(incomeItem.Id);
                        //edit items
                        DB.IncomeItems.Attach(incomeItem);
                        DB.Entry(incomeItem).State = EntityState.Modified;
                    }
                }

                if (itemDb.IncomeItems != null)
                {//delete removed items
                    foreach (var incomeItem in itemDb.IncomeItems)
                    {
                        if (!itemsMap.Contains(incomeItem.Id))
                        {
                            incomeItem.Income = null;
                            DB.IncomeItems.Remove(incomeItem);
                        }
                    }
                }
            }
        }

        await DB.SaveChangesAsync();
        DB.Entry(item).State = EntityState.Detached;

        return item.Id;
    }

    /// <summary>
    /// Удалить элемент
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public override async Task DeleteItem(long id)
    {
        var item = await entities
            .Include(x => x.IncomeItems)
            .AsNoTracking()
            .FirstAsync(x => x.Id == id);

        entities.Remove(item);
        await DB.SaveChangesAsync();
    }
}
