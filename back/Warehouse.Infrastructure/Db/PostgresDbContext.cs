using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db;

public class PostgresDbContext : DbContext
{
    DbSet<ResourceEntity> resources { get; set; }

    DbSet<UnitEntity> units { get; set; }

    DbSet<ClientEntity> clients { get; set; }

    DbSet<BalanceEntity> balances { get; set; }

    DbSet<IncomeEntity> incomes { get; set; }

    public DbSet<IncomeItemEntity> incomeItems { get; set; }

    DbSet<ShipmentEntity> shipments { get; set; }

    public DbSet<ShipmentItemEntity> shipmetItems { get; set; }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
