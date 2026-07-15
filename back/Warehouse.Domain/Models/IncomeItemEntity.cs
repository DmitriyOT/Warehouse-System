using System.ComponentModel.DataAnnotations;
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
    [Required(ErrorMessage = "Не выбран ресурс.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор ресурса должен быть положительным.")]
    public long ResourceId { get; set; }

    /// <summary>
    /// Единица измерения ресурса
    /// </summary>
    public virtual UnitEntity? Unit { get; set; }
    /// <summary>
    /// Единица измерения ресурса
    /// </summary>
    [Required(ErrorMessage = "Не выбрана единица измерения.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор единицы измерения должен быть положительным.")]
    public long UnitId { get; set; }

    /// <summary>
    /// Количество ресурса
    /// </summary>
    [Required(ErrorMessage = "Не указано количество.")]
    [Range(1, long.MaxValue, ErrorMessage = "Количество должно быть положительным.")]
    public long Quantity { get; set; }
}
