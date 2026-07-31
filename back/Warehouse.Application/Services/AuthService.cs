using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Warehouse.Contracts.Application;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastructure;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Services;

/// <summary>
/// Сервис аутентификации: проверка логина/пароля (BCrypt) и выдача JWT-токена
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="configuration"></param>
    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public async Task<string> Login(string login, string password)
    {
        var user = await _userRepository.GetByLogin(login);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new UserException("Неверный логин или пароль");
        }

        return GenerateToken(user);
    }

    /// <inheritdoc />
    public async Task SeedAdmin(string login, string password)
    {
        if (await _userRepository.HasAnyUser())
        {
            return;
        }

        var user = new UserEntity
        {
            Login = login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };
        await _userRepository.Add(user);
    }

    /// <summary>
    /// Сгенерировать JWT-токен (HS256) для пользователя
    /// </summary>
    private string GenerateToken(UserEntity user)
    {
        var key = _configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException(
                "Ключ подписи JWT не задан. Укажите 'Jwt:Key' в конфигурации " +
                "или переменную окружения 'Jwt__Key'.");
        }

        var expiresMinutes = double.TryParse(_configuration["Jwt:ExpiresMinutes"], out var minutes)
            ? minutes
            : 60;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
