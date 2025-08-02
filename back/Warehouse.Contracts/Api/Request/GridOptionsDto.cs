namespace Warehouse.Contracts.Api.Request;

/// <summary>
/// Параметры грида
/// </summary>
public class GridOptionsDto
{
    /// <summary>
    /// Номер страницы, начиная с 1-ой
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Размер страницы
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Строка поиска по всем полям
    /// </summary>
    public string? Search { get; set; }
    /// <summary>
    /// Фильтры для грида
    /// </summary>
    public List<FilterDto>? Filters { get; set; }

    /// <summary>
    /// Перерасчёт пагинации
    /// </summary>
    /// <returns></returns>
    public int GetSkip()
    {
        return (Page - 1) * PageSize;
    }

    /// <summary>
    /// Перерасчёт пагинации
    /// </summary>
    /// <returns></returns>
    public int GetTake()
    {
        return PageSize;
    }
}
