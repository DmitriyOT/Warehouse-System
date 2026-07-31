using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Infrastructure;

/// <summary>
/// Репозиторий пользователей
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Найти пользователя по логину
    /// </summary>
    /// <param name="login">Логин</param>
    /// <returns>Пользователь или null, если не найден</returns>
    Task<UserEntity?> GetByLogin(string login);

    /// <summary>
    /// Есть ли в системе хотя бы один пользователь
    /// </summary>
    Task<bool> HasAnyUser();

    /// <summary>
    /// Добавить пользователя
    /// </summary>
    /// <param name="user">Пользователь</param>
    Task Add(UserEntity user);
}
