using System.ComponentModel.DataAnnotations;

namespace Warehouse.Domain.Models.Base;

/// <summary>
/// Базовый класс с ID, именем и признаком архива для реализации репозиториев
/// </summary>
public class BaseEntityWithIdArchiveName : BaseEntityWithId
{
    /// <summary>
    /// Помещён ли в архив объект
    /// </summary>
    public bool IsArchive { get; set; }

    /// <summary>
    /// Наименование объекта
    /// </summary>
    [Required(ErrorMessage = "Не указано наименование.")]
    [MaxLength(200, ErrorMessage = "Наименование не может превышать 200 символов.")]
    public required string Name { get; set; }
}
