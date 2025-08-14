using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Infrastracture;

/// <summary>
/// Репозиторий для поступлений
/// </summary>
public interface IIncomeRepository : ICrudRepository<IncomeEntity>
{
}
