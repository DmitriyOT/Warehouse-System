using System.ComponentModel.DataAnnotations;

namespace Warehouse.Contracts.Api.Request.Dtos;

/// <summary>
/// Дто запроса на вход в систему
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Логин
    /// </summary>
    [Required]
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}
