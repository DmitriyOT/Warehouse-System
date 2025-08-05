using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Единица поступления
/// </summary>
public class IncomeItemEntity : BaseEntityWithId
{
    /// <summary>
    /// Документ поступления
    /// </summary>
    public required virtual IncomeEntity Income { get; set; }

    /// <summary>
    /// Ресурс
    /// </summary>
    public required virtual ResourceEntity Resource { get; set; }

    /// <summary>
    /// Единица измерения ресурса
    /// </summary>
    public required virtual UnitEntity Unit { get; set; }

    /// <summary>
    /// Количетсво ресурса
    /// </summary>
    public long Quantity { get; set; }
}
