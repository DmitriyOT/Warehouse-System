using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db.ModelsConfiguration;

public class BalanceEntityConfiguration : IEntityTypeConfiguration<BalanceEntity>
{
    public void Configure(EntityTypeBuilder<BalanceEntity> builder)
    {
        builder.HasIndex(nameof(BalanceEntity.ResourceId), nameof(BalanceEntity.UnitId)).IsUnique();
        builder.ToTable(x => x.HasCheckConstraint("ValidQuantity", "\"Quantity\" > 0"));
    }
}
