using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Infrastracture;

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
}
