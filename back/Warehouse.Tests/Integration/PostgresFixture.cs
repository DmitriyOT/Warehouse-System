using Testcontainers.PostgreSql;

namespace Warehouse.Tests.Integration;

/// <summary>
/// Общая фикстура: контейнер PostgreSQL 16 на весь набор интеграционных и API-тестов.
/// Если Docker недоступен, контейнер не поднимается, а тесты скипаются через <see cref="IsAvailable"/>.
/// </summary>
public class PostgresFixture : IAsyncLifetime
{
    private PostgreSqlContainer? _container;

    /// <summary>
    /// Строка подключения к БД postgres внутри контейнера (пустая, если контейнер не поднялся)
    /// </summary>
    public string ConnectionString { get; private set; } = string.Empty;

    /// <summary>
    /// Поднялся ли контейнер (Docker доступен)
    /// </summary>
    public bool IsAvailable => _container != null;

    /// <summary>
    /// Причина, по которой контейнер не поднялся (для сообщения скипа)
    /// </summary>
    public string UnavailableReason { get; private set; } = "Контейнер PostgreSQL не запущен.";

    public async Task InitializeAsync()
    {
        try
        {
            _container = new PostgreSqlBuilder()
                .WithImage("postgres:16-alpine")
                .Build();
            await _container.StartAsync();
            ConnectionString = _container.GetConnectionString();
        }
        catch (Exception ex)
        {
            // Docker-демон недоступен или образ не удалось скачать — тесты будут заскипаны
            _container = null;
            UnavailableReason = $"Docker/Testcontainers недоступен: {ex.GetType().Name}: {ex.Message}";
        }
    }

    public async Task DisposeAsync()
    {
        if (_container != null)
        {
            await _container.DisposeAsync();
        }
    }
}
