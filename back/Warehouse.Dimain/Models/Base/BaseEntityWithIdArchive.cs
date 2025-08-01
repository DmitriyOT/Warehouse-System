namespace Warehouse.Domain.Models.Base;

/// <summary>
/// Базовый класс с ID и признаком архива для реализации репозиториев
/// </summary>
public class BaseEntityWithIdArchive : BaseEntityWithId
{
    /// <summary>
    /// Помещён ли в архив объект
    /// </summary>
    public bool IsArchive { get; set; }
}
