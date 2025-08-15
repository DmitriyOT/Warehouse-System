using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Application;

/// <summary>
/// Сервис для работы с балансом склада
/// </summary>
public interface IBalanceService : ICrudService<BalanceEntity>
{
    /// <summary>
    /// Применить изменения для баланса
    /// </summary>
    /// <returns></returns>
    public Task ApplyIncomeDifference(ICollection<IncomeItemEntity> items);

    /// <summary>
    /// Применить изменения для баланса
    /// </summary>
    /// <returns></returns>
    public Task ApplyShipmentDifference(ICollection<ShipmentItemEntity> items);

    /// <summary>
    /// Посчитать и применить изменения баланса
    /// </summary>
    /// <param name="itemsOld"></param>
    /// <param name="itemsNow"></param>
    /// <returns></returns>
    public Task CalculateAndApplyDifference(ICollection<IncomeItemEntity> itemsOld, ICollection<IncomeItemEntity> itemsNow);
    /// <summary>
    /// Посчитать и применить изменения баланса
    /// </summary>
    /// <param name="itemsOld"></param>
    /// <param name="itemsNow"></param>
    /// <returns></returns>
    public Task CalculateAndApplyDifference(ICollection<ShipmentItemEntity> itemsOld, ICollection<ShipmentItemEntity> itemsNow);
}
