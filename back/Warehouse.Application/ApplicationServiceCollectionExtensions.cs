using Microsoft.Extensions.DependencyInjection;
using Warehouse.Application.Services;
using Warehouse.Application.Services.Base;
using Warehouse.Contracts.Application;

namespace Warehouse.Application;

/// <summary>
/// Методы расширения для регистрации прикладных сервисов
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Добавить бизнес-сервисы приложения
    /// </summary>
    public static IServiceCollection AddWarehouseApplication(this IServiceCollection services)
    {
        // Универсальные CRUD-сервисы
        services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
        services.AddScoped(typeof(IArchiveCrudService<>), typeof(ArchiveCrudService<>));

        // Специализированные сервисы
        services.AddScoped<IncomeService>();
        services.AddScoped<ShipmentService>();
        services.AddScoped<IBalanceService, BalanceService>();

        return services;
    }
}
