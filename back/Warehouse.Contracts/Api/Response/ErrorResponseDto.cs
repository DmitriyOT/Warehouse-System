using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Contracts.Api.Response;

/// <summary>
/// Дто для возврата ошибок
/// </summary>
public class ErrorResponseDto : ResponseDto<object?>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="ex">Исключение</param>
    public ErrorResponseDto(Exception ex) : base(null)
    {
        HasError = true;
#if DEBUG
        ErrorMessage = ex.ToString();
#else
        ErrorMessage = ex.Message;
#endif
    }
}
