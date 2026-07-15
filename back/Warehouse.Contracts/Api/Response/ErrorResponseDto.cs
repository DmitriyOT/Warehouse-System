namespace Warehouse.Contracts.Api.Response;

/// <summary>
/// Дто для возврата ошибок
/// </summary>
public class ErrorResponseDto : ResponseDto<object?>
{
    /// <summary>
    /// Конструктор на основе исключения
    /// </summary>
    /// <param name="ex">Исключение</param>
    /// <param name="includeStackTrace">Включить stack trace (только для DEBUG)</param>
    public ErrorResponseDto(Exception ex, bool includeStackTrace = false) : base(null)
    {
        HasError = true;
#if DEBUG
        ErrorMessage = includeStackTrace ? ex.ToString() : ex.Message;
#else
        ErrorMessage = ex.Message;
#endif
    }

    /// <summary>
    /// Конструктор на основе сообщения
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    public ErrorResponseDto(string message) : base(null)
    {
        HasError = true;
        ErrorMessage = message;
    }
}
