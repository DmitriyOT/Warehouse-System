using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Db.Repository.Base;

namespace Warehouse.Infrastructure.Db.Repository;

public class ShipmentRepository : CrudRepository<ShipmentEntity>, IShipmentRepository
{
    public ShipmentRepository(PostgresDbContext db) : base(db)
    {
    }

    /// <summary>
    /// Получить элемент без отслеживания изменений
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public override async Task<ShipmentEntity> GetItem(long id)
    {
        var item = await entities
            .Include(x => x.ShipmentItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        if (item == null)
        {
            throw new Exception($"Error. Item with this Id={id} not found in Database.");
        }
        else
        {
            return item;
        }
    }

    public override async Task<Tuple<List<ShipmentEntity>, long>> GetAll(GridOptionsDto options)
    {
        var query = GetQuery(options);
        return Tuple.Create(
            await query.OrderBy(x => x.Id)
                .Skip(options.GetSkip()).Take(options.GetTake())//Paginations
                .Include(x => x.ShipmentItems).ThenInclude(x => x.Resource)
                .Include(x => x.ShipmentItems).ThenInclude(x => x.Unit)
                .Select(x => new ShipmentEntity
                {
                    Id = x.Id,
                    Number = x.Number,
                    ClientName = x.Client.Name,
                    Date = x.Date,
                    IsApprove = x.IsApprove,
                    ShipmentItems = x.ShipmentItems
                })
                .AsNoTracking().ToListAsync(),//To array (List)
            await query.LongCountAsync()
            );
    }

    public override async Task<long> EditItem(ShipmentEntity item)
    {
        var itemDb = await entities
            .Include(x => x.ShipmentItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        if (itemDb == null)
        {//Create New
            if (item.Id < 0)//Отрицательные id не используем, 0 тогда создастся корректный id
                item.Id = 0;

            entities.Add(item);

            if (item.ShipmentItems != null)
            {
                foreach (var incomeItem in item.ShipmentItems)
                {
                    incomeItem.Id = 0;
                    incomeItem.Shipment = item;
                }
                DB.shipmetItems.AddRange(item.ShipmentItems);
            }
        }
        else
        {//edit exist
            entities.Attach(item);
            DB.Entry(item).State = EntityState.Modified;
            if (item.ShipmentItems != null)
            {
                HashSet<long> itemsMap = new HashSet<long>();
                foreach (var incomeItem in item.ShipmentItems)
                {
                    incomeItem.Shipment = item;
                    if (incomeItem.Id < 0)
                    {//add new items
                        incomeItem.Id = 0;
                        DB.shipmetItems.Add(incomeItem);
                    }
                    else
                    {//add to set exist items
                        itemsMap.Add(incomeItem.Id);
                        //edit items
                        DB.shipmetItems.Attach(incomeItem);
                        DB.Entry(incomeItem).State = EntityState.Modified;
                    }
                }

                if (itemDb.ShipmentItems != null)
                {//delete removed items
                    foreach (var incomeItem in itemDb.ShipmentItems)
                    {
                        if(!itemsMap.Contains(incomeItem.Id))
                        {
                            incomeItem.Shipment = null;
                            DB.shipmetItems.Remove(incomeItem);
                        }
                    }
                }
            }
        }

        await DB.SaveChangesAsync();
        DB.Entry(item).State = EntityState.Detached;
        return item.Id;
    }
}
