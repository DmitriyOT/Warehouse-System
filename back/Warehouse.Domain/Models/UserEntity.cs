using Warehouse.Domain.Models.Base;

namespace Warehouse.Domain.Models;

/// <summary>
/// Пользователь системы
/// </summary>
public class UserEntity : BaseEntityWithId
{
    /// <summary>
    /// Логин пользователя (уникальный)
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Хэш пароля (BCrypt)
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
}
