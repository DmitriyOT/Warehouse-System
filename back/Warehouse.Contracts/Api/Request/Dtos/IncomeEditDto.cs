using System.ComponentModel.DataAnnotations;

namespace Warehouse.Contracts.Api.Request.Dtos;

/// <summary>
/// DTO для создания/редактирования документа поступления
/// </summary>
public class IncomeEditDto
{
    /// <summary>
    /// Идентификатор документа (0 — новый документ)
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Номер документа
    /// </summary>
    [Required(ErrorMessage = "Не указан номер документа.")]
    [MaxLength(50, ErrorMessage = "Номер документа не может превышать 50 символов.")]
    public required string Number { get; set; }

    /// <summary>
    /// Дата поступления
    /// </summary>
    [Required(ErrorMessage = "Не указана дата поступления.")]
    public DateOnly Date { get; set; }

    /// <summary>
    /// Строки поступления
    /// </summary>
    [Required(ErrorMessage = "Документ должен содержать хотя бы одну строку.")]
    [MinLength(1, ErrorMessage = "Документ должен содержать хотя бы одну строку.")]
    public required List<IncomeItemEditDto> IncomeItems { get; set; }
}
