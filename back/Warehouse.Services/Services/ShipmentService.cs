using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Application.Services.Base;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Services;

public class ShipmentService : CrudService<ShipmentEntity>
{
    public ShipmentService(IShipmentRepository repository) : base(repository)
    {
    }

    public override Task<long> EditItem(ShipmentEntity item)
    {
        if (item.ShipmentItems != null)
        {
            foreach (var elem in item.ShipmentItems)
            {
                if (elem.Quantity <= 0)
                {
                    throw new UserException("Ошибка. Количество ресурса должно быть положительным в документе отгрузки.");
                }
            }
        }

        return base.EditItem(item);
    }
}
