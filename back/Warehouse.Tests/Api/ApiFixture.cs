using Warehouse.Tests.Integration;

namespace Warehouse.Tests.Api;

/// <summary>
/// Фикстура API-тестов: изолированная БД в общем контейнере + тестовый хост приложения.
/// Миграции и seed администратора выполняет само приложение при старте (Program.Main).
/// </summary>
public class ApiFixture : IAsyncLifetime
{
    private readonly PostgresFixture _postgres;
    private readonly Dictionary<string, string?> _savedEnvironment = new();
    private IsolatedDatabase? _database;
    private WarehouseApiFactory? _factory;

    public ApiFixture(PostgresFixture postgres)
    {
        _postgres = postgres;
    }

    /// <summary>
    /// HttpClient на тестовый хост (null, если Docker недоступен)
    /// </summary>
    public HttpClient? Client { get; private set; }

    /// <summary>
    /// Доступен ли тестовый хост (поднялся ли контейнер PostgreSQL)
    /// </summary>
    public bool IsAvailable => _postgres.IsAvailable;

    /// <summary>
    /// Причина недоступности (для сообщения скипа)
    /// </summary>
    public string UnavailableReason => _postgres.UnavailableReason;

    public async Task InitializeAsync()
    {
        if (!_postgres.IsAvailable)
        {
            return;
        }

        _database = await IsolatedDatabase.CreateAsync(_postgres);

        // Конфигурацию подменяем переменными окружения, а не ConfigureAppConfiguration
        // фабрики: источники фабрики добавляются поздно, и код Program.Main,
        // читающий builder.Configuration до Build() (строка подключения, Jwt:Key),
        // их не видит — из-за этого тестовый хост цеплялся к ЛОКАЛЬНОЙ базе
        // из appsettings.Development.json и подписывал токен другим ключом.
        // Переменные окружения WebApplication.CreateBuilder читает сразу.
        SetEnvironment("ConnectionStrings__DefaultConnection", _database.ConnectionString);
        SetEnvironment("AdminUser__Login", WarehouseApiFactory.AdminLogin);
        SetEnvironment("AdminUser__Password", WarehouseApiFactory.AdminPassword);
        SetEnvironment("Jwt__Key", "integration-tests-jwt-key-0123456789abcdef");

        _factory = new WarehouseApiFactory(_database.ConnectionString);
        // CreateClient поднимает хост: Program.Main применяет миграции и сеет админа
        Client = _factory.CreateClient();
    }

    private void SetEnvironment(string name, string value)
    {
        _savedEnvironment[name] = Environment.GetEnvironmentVariable(name);
        Environment.SetEnvironmentVariable(name, value);
    }

    private void RestoreEnvironment()
    {
        foreach (var (name, value) in _savedEnvironment)
        {
            Environment.SetEnvironmentVariable(name, value);
        }
        _savedEnvironment.Clear();
    }

    public async Task DisposeAsync()
    {
        Client?.Dispose();
        if (_factory != null)
        {
            await _factory.DisposeAsync();
        }
        RestoreEnvironment();
        if (_database != null)
        {
            await _database.DisposeAsync();
        }
    }
}
