using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Клиент склада
/// </summary>
public class ClientEntity : BaseEntityWithIdArchiveName
{
    /// <summary>
    /// Адрес клиента
    /// </summary>
    [Required(ErrorMessage = "Не указан адрес клиента.")]
    [MaxLength(500, ErrorMessage = "Адрес не может превышать 500 символов.")]
    public required string Address { get; set; }

    /// <summary>
    /// Навигационное свойство отгрузок
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<ShipmentEntity>? Shipments { get; set; }
}
