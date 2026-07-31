using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Warehouse.Api;

namespace Warehouse.Tests.Api;

/// <summary>
/// Фабрика тестового хоста Warehouse.Api.
/// Подменяет строку подключения на PostgreSQL из контейнера Testcontainers
/// и задаёт креды seed-администратора (Program при старте сам применяет миграции и сеет админа).
/// </summary>
public class WarehouseApiFactory : WebApplicationFactory<Program>
{
    /// <summary>
    /// Логин seed-администратора в тестовом хосте
    /// </summary>
    public const string AdminLogin = "testadmin";

    /// <summary>
    /// Пароль seed-администратора в тестовом хосте
    /// </summary>
    public const string AdminPassword = "test-admin-password-123";

    private readonly string _connectionString;

    public WarehouseApiFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _connectionString,
                ["AdminUser:Login"] = AdminLogin,
                ["AdminUser:Password"] = AdminPassword,
                ["Jwt:Key"] = "integration-tests-jwt-key-0123456789abcdef",
                ["Jwt:Issuer"] = "Warehouse.Api",
                ["Jwt:Audience"] = "Warehouse.Client"
            });
        });
    }
}
