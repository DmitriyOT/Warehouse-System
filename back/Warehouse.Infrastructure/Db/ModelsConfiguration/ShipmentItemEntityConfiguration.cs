using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db.ModelsConfiguration;

public class ShipmentItemEntityConfiguration : IEntityTypeConfiguration<ShipmentItemEntity>
{
    public void Configure(EntityTypeBuilder<ShipmentItemEntity> builder)
    {
        builder.HasIndex(nameof(ShipmentItemEntity.Id), nameof(ShipmentItemEntity.ResourceId), nameof(ShipmentItemEntity.UnitId)).IsUnique();
        builder.ToTable(x => x.HasCheckConstraint("ValidQuantity", "\"Quantity\" > 0"));
    }
}
