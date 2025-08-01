namespace Warehouse.Contracts.Api.Response;

/// <summary>
/// Класс для простых возвращаемых данных
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResponseDto<T>
{
    /// <summary>
    /// Возвращаемые данные, если не ошибка
    /// </summary>
    public T Response { get; protected set; }

    /// <summary>
    /// Возврат с ошибкой или нет?
    /// </summary>
    public bool HasError { get; protected set; }

    /// <summary>
    /// Тест ошибки, если ошибка
    /// </summary>
    public string? ErrorMessage { get; protected set; }

    /// <summary>
    /// Конструктор для хорошего результата
    /// </summary>
    public ResponseDto(T reponse)
    {
        Response = reponse;

        HasError = false;
        ErrorMessage = null;
    }
}
