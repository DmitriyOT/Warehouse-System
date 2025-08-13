using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Infrastracture;

/// <summary>
/// Репозиторий для поступлений
/// </summary>
public interface IIncomeRepository : ICrudRepository<IncomeEntity>
{
}
