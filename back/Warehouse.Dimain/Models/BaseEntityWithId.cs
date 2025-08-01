namespace Warehouse.Domain.Models;

/// <summary>
/// Базовый класс с ID для реализации репозиториев
/// </summary>
public class BaseEntityWithId
{
    public long Id { get; set; }
}
