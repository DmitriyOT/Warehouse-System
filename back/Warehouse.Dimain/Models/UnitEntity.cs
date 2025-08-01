using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Единица измерения, шт. кг. и тп.
/// </summary>
public class UnitEntity : BaseEntityWithIdArchive
{
    /// <summary>
    /// Наименование
    /// </summary>
    public required string Name { get; set; }
}
