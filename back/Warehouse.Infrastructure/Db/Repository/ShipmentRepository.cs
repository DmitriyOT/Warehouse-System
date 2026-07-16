using Microsoft.EntityFrameworkCore;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Api.Response;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastructure;
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
            .Include(x => x.ShipmentItems).ThenInclude(x => x.Resource)
            .Include(x => x.ShipmentItems).ThenInclude(x => x.Unit)
            .AsNoTracking()
            .Select(x => new ShipmentEntity
            {
                Id = x.Id,
                Number = x.Number,
                ClientName = x.Client.Name,
                Date = x.Date,
                IsApprove = x.IsApprove,
                ShipmentItems = x.ShipmentItems,
                ClientId = x.ClientId
            })
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

    public override async Task<PagedResult<ShipmentEntity>> GetAll(GridOptionsDto options)
    {
        var query = GetQuery(options);
        var items = await query.OrderBy(x => x.Id)
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
            .AsNoTracking().ToListAsync();//To array (List)
        var count = await query.LongCountAsync();
        return new PagedResult<ShipmentEntity>(items, count);
    }

    public override async Task<long> EditItem(ShipmentEntity item)
    {
        var numberEqual = await entities.AsNoTracking().FirstOrDefaultAsync(x => x.Number == item.Number);
        if (numberEqual != null && numberEqual.Id != item.Id)
        {
            throw new UserException($"Ошибка. Документ отгрузки с номером {item.Number} уже есть в системе.");
        }

        if (item.ShipmentItems != null)
        {
            foreach (var shipmentItem in item.ShipmentItems)
            {
                shipmentItem.Resource = null;
                shipmentItem.Unit = null;
            }
        }

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
                foreach (var shipmentItem in item.ShipmentItems)
                {
                    shipmentItem.Id = 0;
                    shipmentItem.Shipment = item;
                }
                DB.ShipmentItems.AddRange(item.ShipmentItems);
            }
        }
        else
        {//edit exist
            entities.Attach(item);
            DB.Entry(item).State = EntityState.Modified;
            if (item.ShipmentItems != null)
            {
                HashSet<long> itemsMap = new HashSet<long>();
                foreach (var shipmentItem in item.ShipmentItems)
                {
                    shipmentItem.Shipment = item;
                    if (shipmentItem.Id < 0)
                    {//add new items
                        shipmentItem.Id = 0;
                        DB.ShipmentItems.Add(shipmentItem);
                    }
                    else
                    {//add to set exist items
                        itemsMap.Add(shipmentItem.Id);
                        //edit items
                        DB.ShipmentItems.Attach(shipmentItem);
                        DB.Entry(shipmentItem).State = EntityState.Modified;
                    }
                }

                if (itemDb.ShipmentItems != null)
                {//delete removed items
                    foreach (var shipmentItem in itemDb.ShipmentItems)
                    {
                        if (!itemsMap.Contains(shipmentItem.Id))
                        {
                            shipmentItem.Shipment = null;
                            DB.ShipmentItems.Remove(shipmentItem);
                        }
                    }
                }
            }
        }

        await DB.SaveChangesAsync();
        DB.Entry(item).State = EntityState.Detached;
        return item.Id;
    }

    public async Task ChangeStateAsync(long id, string newStateCode)
    {
        var item = await entities.Include(x => x.ShipmentItems).FirstAsync(x => x.Id == id);
        item.IsApprove = newStateCode == "approve";

        await DB.SaveChangesAsync();
        DB.Entry(item).State = EntityState.Detached;
    }
}
