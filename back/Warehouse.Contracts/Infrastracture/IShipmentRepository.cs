using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Infrastracture;

/// <summary>
/// Репозиторий для отгрузок
/// </summary>
public interface IShipmentRepository : ICrudRepository<ShipmentEntity>
{
}
