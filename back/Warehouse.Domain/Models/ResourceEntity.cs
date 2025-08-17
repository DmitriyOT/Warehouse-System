using System.Text.Json.Serialization;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Ресурс на складе
/// </summary>
public class ResourceEntity : BaseEntityWithIdArchiveName
{

    /// <summary>
    /// Навигационное свойство поступлений
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<IncomeItemEntity>? IncomeItems { get; set; }

    /// <summary>
    /// Навигационное свойство отгрузок
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<ShipmentItemEntity>? ShipmentItems { get; set; }

    /// <summary>
    /// Навигационное свойство баланса
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<BalanceEntity>? BalanceItems { get; set; }
}
