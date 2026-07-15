using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Infrastructure.Db;
using Warehouse.Infrastructure.Db.Repository;
using Warehouse.Infrastructure.Db.Repository.Base;
using Warehouse.Contracts.Infrastracture;

namespace Warehouse.Infrastructure;

/// <summary>
/// Методы расширения для регистрации инфраструктурных сервисов
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Добавить инфраструктуру: контекст БД, репозитории, единицу работы и health checks
    /// </summary>
    public static IServiceCollection AddWarehouseInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? configuration.GetSection("ConnectionString").Value;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Строка подключения к PostgreSQL не найдена. " +
                "Укажите 'ConnectionStrings:DefaultConnection' или 'ConnectionString' в конфигурации.");
        }

        // Закладываем запас производительности, чтобы не исчерпать подключения к БД при высокой нагрузке.
        // По умолчанию у PostgreSQL 100 подключений, поэтому здесь ставим 100.
        services.AddDbContextPool<PostgresDbContext>((_, options) =>
        {
            options.UseNpgsql(connectionString);
        }, poolSize: 100);

        // Универсальные репозитории
        services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));
        services.AddScoped(typeof(IArchiveCrudRepository<>), typeof(ArchiveCrudRepository<>));

        // Специализированные репозитории
        services.AddScoped<IIncomeRepository, IncomeRepository>();
        services.AddScoped<IShipmentRepository, ShipmentRepository>();
        services.AddScoped<IBalanceRepository, BalanceRepository>();

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddHealthChecks()
            .AddNpgSql(
                connectionString,
                name: "postgresql",
                failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);

        return services;
    }
}
