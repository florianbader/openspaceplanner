using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

public abstract class TenantBaseEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity>
    where TEntity : class, IBaseEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        // Tenant base entities are multi-tenant by default
        builder.IsMultiTenant();
    }
}
