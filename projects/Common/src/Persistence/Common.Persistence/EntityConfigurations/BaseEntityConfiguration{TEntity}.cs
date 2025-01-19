using Gridify;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

public abstract class BaseEntityConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>,
        IGridifyConfiguration<TEntity>
    where TEntity : class, IBaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(typeof(TEntity).Name + "s");

        builder.Property(p => p.CreatedBy).HasMaxLength(DataTypes.UserId);
        builder.Property(p => p.LastUpdatedBy).HasMaxLength(DataTypes.UserId);
        builder.Property(p => p.DeletedBy).HasMaxLength(DataTypes.UserId);

        // Use current date and time UTC for CreatedAt and LastUpdatedAt when entity is created.
        builder.Property(p => p.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("getutcdate()");
        builder.Property(p => p.LastUpdatedAt).ValueGeneratedOnUpdate().HasDefaultValueSql("getutcdate()");

        // Filter out soft deleted entities by default
        // as we don't want to manually deal with them everytime
        // and getting them would be an edge case
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);
        builder.HasQueryFilter(p => !p.IsDeleted);

        // Row version to check for concurrent updates
        // Shadow property because we don't care about this in our code.
        builder.Property<byte[]>("RowVersion").IsRowVersion();
    }

    public virtual void Configure(GridifyMapper<TEntity> mapper) { }
}
