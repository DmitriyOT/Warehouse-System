using System.Text.Json.Serialization;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Клиент склада
/// </summary>
public class ClientEntity : BaseEntityWithIdArchiveName
{
    /// <summary>
    /// Адресс клиента
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Навигационное свойство отгрузок
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<ShipmentEntity>? Shipments { get; set; }
}
