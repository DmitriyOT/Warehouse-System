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

    DbSet<IncomeItemEntity> incomeItems { get; set; }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
