using System.ComponentModel.DataAnnotations;

namespace Warehouse.Contracts.Api.Request.Dtos;

/// <summary>
/// DTO для создания/редактирования строки поступления
/// </summary>
public class IncomeItemEditDto
{
    /// <summary>
    /// Идентификатор строки (0 — новая строка)
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Идентификатор ресурса
    /// </summary>
    [Required(ErrorMessage = "Не выбран ресурс.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор ресурса должен быть положительным.")]
    public long ResourceId { get; set; }

    /// <summary>
    /// Идентификатор единицы измерения
    /// </summary>
    [Required(ErrorMessage = "Не выбрана единица измерения.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор единицы измерения должен быть положительным.")]
    public long UnitId { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    [Required(ErrorMessage = "Не указано количество.")]
    [Range(1, long.MaxValue, ErrorMessage = "Количество должно быть положительным.")]
    public long Quantity { get; set; }
}
