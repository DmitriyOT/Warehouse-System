using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Infrastracture;

/// <summary>
/// Репозиторий для отгрузок
/// </summary>
public interface IShipmentRepository : ICrudRepository<ShipmentEntity>
{
    /// <summary>
    /// Изменение состояния отгрузки, подписана или не подписана
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newStateCode"></param>
    /// <returns></returns>
    public Task ChangeStateAsync(long id, string newStateCode);
}
