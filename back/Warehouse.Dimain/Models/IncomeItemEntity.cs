using System.Text.Json.Serialization;
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
    [JsonIgnore]
    public virtual IncomeEntity? Income { get; set; }

    /// <summary>
    /// Ресурс
    /// </summary>
    public virtual ResourceEntity? Resource { get; set; }
    /// <summary>
    /// Ресурс
    /// </summary>
    public long ResourceId { get; set; }

    /// <summary>
    /// Единица измерения ресурса
    /// </summary>
    public virtual UnitEntity? Unit { get; set; }
    /// <summary>
    /// Единица измерения ресурса
    /// </summary>
    public long UnitId { get; set; }

    /// <summary>
    /// Количетсво ресурса
    /// </summary>
    public long Quantity { get; set; }
}
