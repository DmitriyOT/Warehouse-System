using System.ComponentModel.DataAnnotations;

namespace Warehouse.Contracts.Api.Request;

/// <summary>
/// Параметры грида
/// </summary>
public class GridOptionsDto
{
    /// <summary>
    /// Номер страницы, начиная с 1-ой
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Номер страницы должен быть не меньше 1.")]
    public int Page { get; set; }
    /// <summary>
    /// Размер страницы, максимум 200
    /// </summary>
    [Range(1, 200, ErrorMessage = "Размер страницы должен быть от 1 до 200.")]
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
