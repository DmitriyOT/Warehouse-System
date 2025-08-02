using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db.ModelsConfiguration;

public class UnitEntityConfiguration : IEntityTypeConfiguration<UnitEntity>
{
    public void Configure(EntityTypeBuilder<UnitEntity> builder)
    {
        builder.HasIndex(e => e.Name).IsUnique();
    }
}
