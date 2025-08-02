namespace Warehouse.Contracts.Api.Response;

/// <summary>
/// Возврат данных для фронта, для грида с пагинацией
/// </summary>
public class ResponseDtoGrid<T> : ResponseDto<GridResponsePair<T>>
{
    /// <summary>
    /// Конструктор со списоком данных и объектом пагинации
    /// </summary>
    public ResponseDtoGrid(List<T> response, PageView page) : base(new GridResponsePair<T> { Items = response, Page = page })
    {
    }
}


/// <summary>
/// Класс для более удобной типизации возвратов
/// </summary>
/// <typeparam name="T"></typeparam>
public class GridResponsePair<T>
{
    /// <summary>
    /// Пагинация
    /// </summary>
    public required PageView Page { get; set; }

    /// <summary>
    /// Элементы
    /// </summary>
    public required List<T> Items { get; set; }
}
