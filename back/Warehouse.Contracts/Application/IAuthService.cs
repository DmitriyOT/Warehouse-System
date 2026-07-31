namespace Warehouse.Contracts.Application;

/// <summary>
/// Сервис аутентификации пользователей
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Выполнить вход по логину и паролю
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <returns>JWT-токен</returns>
    /// <exception cref="Warehouse.Contracts.Exceptions.UserException">Неверный логин или пароль</exception>
    Task<string> Login(string login, string password);

    /// <summary>
    /// Создать первого пользователя (администратора), если в системе нет ни одного пользователя
    /// </summary>
    /// <param name="login">Логин администратора</param>
    /// <param name="password">Пароль администратора (будет захэширован)</param>
    Task SeedAdmin(string login, string password);
}
