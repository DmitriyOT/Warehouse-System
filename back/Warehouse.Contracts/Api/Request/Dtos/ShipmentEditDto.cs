using System.ComponentModel.DataAnnotations;

namespace Warehouse.Contracts.Api.Request.Dtos;

/// <summary>
/// DTO для создания/редактирования документа отгрузки
/// </summary>
public class ShipmentEditDto
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
    /// Дата отгрузки
    /// </summary>
    [Required(ErrorMessage = "Не указана дата отгрузки.")]
    public DateOnly Date { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    [Required(ErrorMessage = "Не выбран клиент.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор клиента должен быть положительным.")]
    public long ClientId { get; set; }

    /// <summary>
    /// Подписана ли отгрузка
    /// </summary>
    public bool IsApprove { get; set; }

    /// <summary>
    /// Строки отгрузки
    /// </summary>
    [Required(ErrorMessage = "Документ должен содержать хотя бы одну строку.")]
    [MinLength(1, ErrorMessage = "Документ должен содержать хотя бы одну строку.")]
    public required List<ShipmentItemEditDto> ShipmentItems { get; set; }
}
