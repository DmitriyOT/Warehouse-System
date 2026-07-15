using System.ComponentModel.DataAnnotations;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Баланс, сколько всего такого-то ресурса сейчас есть на складе
/// </summary>
public class BalanceEntity : BaseEntityWithId
{
    /// <summary>
    /// Навигационное поле ресурса
    /// </summary>
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор ресурса должен быть положительным.")]
    public long ResourceId { get; set; }

    /// <summary>
    /// Навигационное поле единица измерения
    /// </summary>
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор единицы измерения должен быть положительным.")]
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
    [Range(0, long.MaxValue, ErrorMessage = "Количество не может быть отрицательным.")]
    public long Quantity { get; set; }
}
