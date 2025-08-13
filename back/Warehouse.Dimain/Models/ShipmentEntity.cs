using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Отгрузка со склада
/// </summary>
public class ShipmentEntity : BaseEntityWithId
{
    /// <summary>
    /// Номер
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Дата отгрузки
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Клиент отгрузки
    /// </summary>
    public long ClientId { get; set; }

    /// <summary>
    /// Клиент отгрузки
    /// </summary>
    public virtual ClientEntity? Client { get; set; }

    /// <summary>
    /// Ресурсы отгрузки
    /// </summary>
    public virtual ICollection<ShipmentItemEntity>? ShipmentItems { get; set; }
}
