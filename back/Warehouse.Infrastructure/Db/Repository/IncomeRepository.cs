using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Application;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Db.Repository.Base;

namespace Warehouse.Infrastructure.Db.Repository;

public class IncomeRepository : CrudRepository<IncomeEntity>, IIncomeRepository
{
    protected IBalanceService _balanceService { get; set; }

    public IncomeRepository(PostgresDbContext db, IBalanceService balanceService) : base(db)
    {
        _balanceService = balanceService;
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

    public override async Task<Tuple<List<IncomeEntity>, long>> GetAll(GridOptionsDto options)
    {
        var query = GetQuery(options);
        return Tuple.Create(
            await query.OrderBy(x => x.Id)
                .Include(x => x.IncomeItems).ThenInclude(x => x.Resource)
                .Include(x => x.IncomeItems).ThenInclude(x => x.Unit)
                .Skip(options.GetSkip()).Take(options.GetTake())//Paginations
                .AsNoTracking().ToListAsync(),//To array (List)
            await query.LongCountAsync()
            );
    }

    public override async Task<long> EditItem(IncomeEntity item)
    {
        var transaction = await DB.Database.BeginTransactionAsync();

        try
        {
            transaction.CreateSavepoint("start");

            var itemDb = await entities
                .Include(x => x.IncomeItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == item.Id);

            if (item.IncomeItems != null)
            {
                foreach (var incomeI in item.IncomeItems)
                {
                    incomeI.Resource = null;
                    incomeI.Unit = null;
                }
            }

            var numberEqual = await entities.Where(x => x.Number == item.Number).AsNoTracking().FirstOrDefaultAsync();
            if (numberEqual != null && numberEqual.Id != item.Id)
            {
                throw new UserException($"Ошибка. Документ поступления с номером <{item.Number}> уже есть в системе.");
            }

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
                    DB.incomeItems.AddRange(item.IncomeItems);
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
                            DB.incomeItems.Add(incomeItem);
                        }
                        else
                        {//add to set exist items
                            itemsMap.Add(incomeItem.Id);
                            //edit items
                            DB.incomeItems.Attach(incomeItem);
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
                                DB.incomeItems.Remove(incomeItem);
                            }
                        }
                    }
                }
            }

            await DB.SaveChangesAsync();
            //await transaction.CommitAsync();
            DB.Entry(item).State = EntityState.Detached;

            if (itemDb != null && item.IncomeItems != null)
            {
                await _balanceService.CalculateAndApplyDifference(
                        itemDb.IncomeItems ?? new List<IncomeItemEntity>(), item.IncomeItems);
            }
            else if (itemDb == null && item.IncomeItems != null)
            {
                await _balanceService.ApplyIncomeDifference(item.IncomeItems);
            }

            await transaction.CommitAsync();
        }
        catch (UserException ex)
        {
            await transaction.RollbackToSavepointAsync("start");
            throw new UserException($"Ошибка обработки документа поступления. {ex.Message}");
        }

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

        try
        {
            if (item.IncomeItems != null)
            {
                foreach (var i in item.IncomeItems)
                {
                    i.Quantity = -i.Quantity;
                }
                await _balanceService.ApplyIncomeDifference(item.IncomeItems);
            }

            entities.Remove(item);
            await DB.SaveChangesAsync();
        }
        catch (UserException ex)
        {
            throw new UserException($"Ошибка удаления поступления. {ex.Message}");
        }
    }
}
