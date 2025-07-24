using Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config.Base;

/// <summary>
/// Configures common auditing and soft-delete properties for auditable entities.
/// </summary>
/// <typeparam name="TEntity">Entity that implements IAuditEntity</typeparam>
public abstract class AuditableEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IAuditableEntity<TKey>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // CreatedBy is required and has a reasonable max length
        builder.Property(e => e.CreatedBy)
               .IsRequired()
               .HasMaxLength(100);

        // CreatedDate is required
        builder.Property(e => e.CreatedDate)
               .IsRequired();

        // UpdatedBy is optional but limited in length
        builder.Property(e => e.UpdatedBy)
               .HasMaxLength(100);

        // UpdatedDate is optional (nullable DateTime), so no config needed unless you want to specify column type

        // Soft delete flag
        builder.Property(e => e.IsDeleted)
               .IsRequired()
               .HasDefaultValue(false);
    }
}