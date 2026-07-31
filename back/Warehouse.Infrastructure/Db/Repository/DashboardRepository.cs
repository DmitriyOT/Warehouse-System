using Microsoft.EntityFrameworkCore;
using Warehouse.Contracts.Infrastructure;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db.Repository;

/// <summary>
/// Репозиторий для агрегирующих запросов дашборда
/// </summary>
public class DashboardRepository : IDashboardRepository
{
    /// <summary>
    /// Контект БД
    /// </summary>
    protected PostgresDbContext DB { get; private set; }

    /// <summary>
    /// Конструктор класса
    /// </summary>
    public DashboardRepository(PostgresDbContext db)
    {
        DB = db ?? throw new ArgumentNullException(nameof(db));
    }

    /// <inheritdoc/>
    public IQueryable<BalanceEntity> Balances => DB.Balances.AsNoTracking();

    /// <inheritdoc/>
    public IQueryable<IncomeEntity> Incomes => DB.Incomes.AsNoTracking();

    /// <inheritdoc/>
    public IQueryable<IncomeItemEntity> IncomeItems => DB.IncomeItems.AsNoTracking();

    /// <inheritdoc/>
    public IQueryable<ShipmentEntity> Shipments => DB.Shipments.AsNoTracking();

    /// <inheritdoc/>
    public IQueryable<ShipmentItemEntity> ShipmentItems => DB.ShipmentItems.AsNoTracking();

    /// <inheritdoc/>
    public IQueryable<ClientEntity> Clients => DB.Clients.AsNoTracking();
}
