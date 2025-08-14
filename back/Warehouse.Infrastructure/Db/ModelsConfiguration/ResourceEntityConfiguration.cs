using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db.ModelsConfiguration;

public class ResourceEntityConfiguration : IEntityTypeConfiguration<ResourceEntity>
{
    public void Configure(EntityTypeBuilder<ResourceEntity> builder)
    {
        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasMany(x => x.IncomeItems).WithOne(x => x.Resource);
        builder.HasMany(x => x.ShipmentItems).WithOne(x => x.Resource);
    }
}
