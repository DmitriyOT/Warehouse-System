using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Application.Services.Base;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Services;

public class ShipmentService : CrudService<ShipmentEntity>
{
    public ShipmentService(IShipmentRepository repository) : base(repository)
    {
    }
}
