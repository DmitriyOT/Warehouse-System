using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db;

public class PostgresDbContext : DbContext
{
    public DbSet<ResourceEntity> Resources { get; set; } = null!;

    public DbSet<UnitEntity> Units { get; set; } = null!;

    public DbSet<ClientEntity> Clients { get; set; } = null!;

    public DbSet<BalanceEntity> Balances { get; set; } = null!;

    public DbSet<IncomeEntity> Incomes { get; set; } = null!;

    public DbSet<IncomeItemEntity> IncomeItems { get; set; } = null!;

    public DbSet<ShipmentEntity> Shipments { get; set; } = null!;

    public DbSet<ShipmentItemEntity> ShipmentItems { get; set; } = null!;

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
