using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Infrastructure;

/// <summary>
/// Репозиторий для поступлений
/// </summary>
public interface IBalanceRepository : ICrudRepository<BalanceEntity>
{
    /// <summary>
    /// Получить элемент баланса по id двух сущностей
    /// </summary>
    /// <param name="resourceId"></param>
    /// <param name="unitId"></param>
    /// <returns></returns>
    public Task<BalanceEntity?> GetBalanceAsync(long resourceId, long unitId);

    /// <summary>
    /// Атомарно применить изменение количества к строке баланса одним UPDATE
    /// </summary>
    /// <param name="resourceId"></param>
    /// <param name="unitId"></param>
    /// <param name="delta">Изменение количества (отрицательное — списание)</param>
    /// <returns>false, если строка баланса не найдена или при отрицательной дельте остаток ушёл бы в минус</returns>
    public Task<bool> TryApplyDeltaAsync(long resourceId, long unitId, long delta);
}
