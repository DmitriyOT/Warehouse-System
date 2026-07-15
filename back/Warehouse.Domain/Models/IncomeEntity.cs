using System.ComponentModel.DataAnnotations;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Поступление на склад
/// </summary>
public class IncomeEntity : BaseEntityWithId
{
    /// <summary>
    /// Номер
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
    /// Ресурсы поступления
    /// </summary>
    public virtual ICollection<IncomeItemEntity>? IncomeItems { get; set; }
}
