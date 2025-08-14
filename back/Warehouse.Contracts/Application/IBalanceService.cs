using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Application;

/// <summary>
/// Сервис для работы с балансом склада
/// </summary>
public interface IBalanceService : ICrudService<BalanceEntity>
{
    /// <summary>
    /// Применить положительные изменения для баланса
    /// </summary>
    /// <returns></returns>
    public Task ApplyIncomeDifference(ICollection<IncomeItemEntity> items);

    /// <summary>
    /// Применить отрицательные изменения для баланса
    /// </summary>
    /// <returns></returns>
    public Task ApplyShipmentDifference(ICollection<ShipmentItemEntity> items);
}
