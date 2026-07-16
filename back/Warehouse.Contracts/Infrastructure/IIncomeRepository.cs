using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Infrastructure;

/// <summary>
/// Репозиторий для поступлений
/// </summary>
public interface IIncomeRepository : ICrudRepository<IncomeEntity>
{
}
