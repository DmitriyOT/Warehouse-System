using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Баланс, сколько всего такого-то ресурса сейчас есть на складе
/// </summary>
public class BalanceEntity : BaseEntityWithId
{
    /// <summary>
    /// Ресурс
    /// </summary>
    public virtual required ResourceEntity Resource { get; set; }

    /// <summary>
    /// Единица измерения
    /// </summary>
    public virtual required UnitEntity Unit { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public long Quantity { get; set; }
}
