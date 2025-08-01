using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Клиент склада
/// </summary>
public class ClientEntity : BaseEntityWithIdArchive
{
    /// <summary>
    /// Наименование
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// Адресс клиента
    /// </summary>
    public required string Address { get; set; }
}
