using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

public abstract class BaseEntityConfiguration<TEntity, TKey> : BaseEntityConfiguration<TEntity>
    where TEntity : BaseEntity<TKey>
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.IsKey(p => p.Id);

        base.Configure(builder);
    }
}
