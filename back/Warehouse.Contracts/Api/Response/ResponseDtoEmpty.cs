namespace Warehouse.Contracts.Api.Response;

/// <summary>
/// Пустое Дто для возврата ничего на фронт
/// </summary>
public class ResponseDtoEmpty : ResponseDto<object?>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    public ResponseDtoEmpty() : base(null)
    {
    }
}
