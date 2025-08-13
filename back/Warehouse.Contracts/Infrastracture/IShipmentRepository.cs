using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Infrastracture;

/// <summary>
/// Репозиторий для отгрузок
/// </summary>
public interface IShipmentRepository : ICrudRepository<ShipmentEntity>
{
}
