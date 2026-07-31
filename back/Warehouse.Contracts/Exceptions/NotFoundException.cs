namespace Warehouse.Contracts.Exceptions;

/// <summary>
/// Ошибка "объект не найден": мапится в 404 Not Found.
/// Наследник UserException, чтобы по-прежнему считаться пользовательской ошибкой
/// </summary>
public class NotFoundException : UserException
{
    /// <summary>
    /// Конструктор базовый
    /// </summary>
    /// <param name="message"></param>
    public NotFoundException(string? message) : base(message)
    {
    }
}
