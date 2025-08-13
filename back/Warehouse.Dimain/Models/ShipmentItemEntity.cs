using System.Text.Json.Serialization;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Единица поступления
/// </summary>
public class ShipmentItemEntity : BaseEntityWithId
{
    /// <summary>
    /// Документ отгрузки
    /// </summary>
    [JsonIgnore]
    public virtual ShipmentEntity? Shipment { get; set; }

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
    /// Количество ресурса
    /// </summary>
    public long Quantity { get; set; }
}
