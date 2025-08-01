using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db;

public class PostgresDbContext : DbContext
{
    DbSet<ResourceEntity> resources { get; set; }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }
}
