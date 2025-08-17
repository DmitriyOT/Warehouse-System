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
    public required string Name { get; set; }
}
