using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Поступление на склад
/// </summary>
public class IncomeEntity : BaseEntityWithId
{
    /// <summary>
    /// Номер
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Дата поступления
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Ресурсы поступления
    /// </summary>
    public virtual ICollection<IncomeItemEntity>? IncomeItems { get; set; }
}
