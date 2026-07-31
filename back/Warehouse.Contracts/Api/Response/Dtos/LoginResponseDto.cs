namespace Warehouse.Contracts.Api.Response.Dtos;

/// <summary>
/// Дто ответа на успешный вход в систему
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// JWT-токен
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="token">JWT-токен</param>
    public LoginResponseDto(string token)
    {
        Token = token;
    }
}
