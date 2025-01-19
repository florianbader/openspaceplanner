using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

public abstract class TenantBaseEntityConfiguration<TEntity, TKey> : TenantBaseEntityConfiguration<TEntity>
    where TEntity : BaseEntity<TKey>
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        // Use the Id property as the primary key
        // and the combination of the tenant and id as the clustered index
        // for better performance when querying data of a tenant
        builder.IsKey(p => p.Id, ["TenantId", "Id"]);
    }
}
