using Microsoft.EntityFrameworkCore;
using Npgsql;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Db;

namespace Warehouse.Tests.Integration;

/// <summary>
/// Изолированная база данных внутри общего контейнера PostgreSQL.
/// Создаёт отдельную БД на тест/класс и удаляет её при освобождении.
/// </summary>
public sealed class IsolatedDatabase : IAsyncDisposable
{
    private readonly string _adminConnectionString;
    private readonly string _databaseName;

    /// <summary>
    /// Строка подключения к изолированной БД
    /// </summary>
    public string ConnectionString { get; }

    private IsolatedDatabase(string adminConnectionString, string databaseName, string connectionString)
    {
        _adminConnectionString = adminConnectionString;
        _databaseName = databaseName;
        ConnectionString = connectionString;
    }

    /// <summary>
    /// Создать новую пустую БД с уникальным именем в контейнере фикстуры
    /// </summary>
    public static async Task<IsolatedDatabase> CreateAsync(PostgresFixture fixture)
    {
        var databaseName = "test_" + Guid.NewGuid().ToString("N");

        await using var connection = new NpgsqlConnection(fixture.ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand($"CREATE DATABASE \"{databaseName}\"", connection);
        await command.ExecuteNonQueryAsync();

        var builder = new NpgsqlConnectionStringBuilder(fixture.ConnectionString)
        {
            Database = databaseName
        };

        return new IsolatedDatabase(fixture.ConnectionString, databaseName, builder.ConnectionString);
    }

    /// <summary>
    /// Применить миграции к изолированной БД
    /// </summary>
    public async Task MigrateAsync()
    {
        await using var context = CreateContext();
        await context.Database.MigrateAsync();
    }

    /// <summary>
    /// Создать новый контекст на изолированной БД (вызывающий код отвечает за Dispose)
    /// </summary>
    public PostgresDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PostgresDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;
        return new PostgresDbContext(options);
    }

    /// <summary>
    /// Создать пару ресурс + единица измерения и вернуть их идентификаторы
    /// </summary>
    public async Task<(long ResourceId, long UnitId)> SeedResourceAndUnitAsync()
    {
        await using var context = CreateContext();
        var resource = new ResourceEntity { Name = "Тестовый ресурс" };
        var unit = new UnitEntity { Name = "шт." };
        context.Resources.Add(resource);
        context.Units.Add(unit);
        await context.SaveChangesAsync();
        return (resource.Id, unit.Id);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await using var connection = new NpgsqlConnection(_adminConnectionString);
            await connection.OpenAsync();
            // WITH (FORCE) завершает открытые подключения к удаляемой БД (PostgreSQL 13+)
            await using var command = new NpgsqlCommand(
                $"DROP DATABASE IF EXISTS \"{_databaseName}\" WITH (FORCE)", connection);
            await command.ExecuteNonQueryAsync();
        }
        catch
        {
            // Освобождение best-effort: контейнер всё равно будет уничтожен фикстурой
        }
    }
}
