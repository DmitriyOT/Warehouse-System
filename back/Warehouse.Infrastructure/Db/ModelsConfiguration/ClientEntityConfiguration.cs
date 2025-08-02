using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db.ModelsConfiguration;

public class ClientEntityConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.HasIndex(e => e.Name).IsUnique();
    }
}
