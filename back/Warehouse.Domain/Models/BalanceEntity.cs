using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Баланс, сколько всего такого-то ресурса сейчас есть на складе
/// </summary>
public class BalanceEntity : BaseEntityWithId
{
    public long ResourceId { get; set; }

    public long UnitId { get; set; }

    /// <summary>
    /// Ресурс
    /// </summary>
    public virtual ResourceEntity? Resource { get; set; }

    /// <summary>
    /// Единица измерения
    /// </summary>
    public virtual UnitEntity? Unit { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public long Quantity { get; set; }
}
