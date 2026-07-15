namespace Warehouse.Contracts.Api.Response;

/// <summary>
/// Результат постраничной выборки
/// </summary>
/// <typeparam name="T">Тип элемента</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Элементы страницы
    /// </summary>
    public List<T> Items { get; init; } = null!;

    /// <summary>
    /// Общее количество элементов
    /// </summary>
    public long TotalCount { get; init; }

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public PagedResult()
    {
    }

    /// <summary>
    /// Конструктор с параметрами
    /// </summary>
    /// <param name="items">Элементы страницы</param>
    /// <param name="totalCount">Общее количество элементов</param>
    public PagedResult(List<T> items, long totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }
}
