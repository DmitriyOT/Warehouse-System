using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db.ModelsConfiguration;

public class IncomeEntityConfiguration : IEntityTypeConfiguration<IncomeEntity>
{
    public void Configure(EntityTypeBuilder<IncomeEntity> builder)
    {
        builder.HasIndex(x => x.Number).IsUnique();
    }
}
