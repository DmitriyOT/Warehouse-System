using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db.ModelsConfiguration;

public class IncomeItemEntityConfiguration : IEntityTypeConfiguration<IncomeItemEntity>
{
    public void Configure(EntityTypeBuilder<IncomeItemEntity> builder)
    {
        builder.HasIndex(nameof(IncomeItemEntity.Id), nameof(IncomeItemEntity.ResourceId), nameof(IncomeItemEntity.UnitId)).IsUnique();
        builder.ToTable(x => x.HasCheckConstraint("ValidQuantity", "\"Quantity\" > 0"));
    }
}
