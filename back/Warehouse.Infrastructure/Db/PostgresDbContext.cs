using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db;

public class PostgresDbContext : DbContext
{
    DbSet<ResourceEntity> resources { get; set; }

    DbSet<UnitEntity> units { get; set; }

    DbSet<ClientEntity> clients { get; set; }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
