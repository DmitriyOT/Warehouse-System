namespace Warehouse.Domain.Models.Base;

/// <summary>
/// Базовый класс с ID для реализации репозиториев
/// </summary>
public class BaseEntityWithId
{
    /// <summary>
    /// Id, уникальный номер сущности
    /// </summary>
    public long Id { get; set; }
}
