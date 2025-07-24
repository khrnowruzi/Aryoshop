using Core.Entities.Products;
using Infrastructure.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config.Products;

public class ProductConfiguration : AuditableEntityConfiguration<Product, Guid>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        // This ensures precision for currency values and avoids floating point errors.
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

        builder.HasIndex(p => new { p.Name, p.Brand, p.Model })
            .IsUnique()
            .HasDatabaseName("IX_Product_Name_Brand_Model");
    }
}