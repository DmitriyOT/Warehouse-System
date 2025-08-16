using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db.ModelsConfiguration;

public class ShipmentEntityConfiguration : IEntityTypeConfiguration<ShipmentEntity>
{
    public void Configure(EntityTypeBuilder<ShipmentEntity> builder)
    {
        builder.HasIndex(x => x.Number).IsUnique();
    }
}
