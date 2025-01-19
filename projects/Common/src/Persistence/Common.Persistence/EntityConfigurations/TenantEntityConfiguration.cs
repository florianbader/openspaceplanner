using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

public class TenantEntityConfiguration : BaseEntityConfiguration<Tenant, TenantId>
{
    public override void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.IsIdentifierText(p => p.Identifier);
        builder.HasIndex(p => p.Identifier).IsUnique();

        builder.IsShortText(p => p.Name);

        // Seed the default tenant
        builder.HasData(
            new Tenant(
                TenantId.Parse("1049efd6-22ab-422a-9bdd-d09a19223f55", CultureInfo.InvariantCulture),
                TenantIdentifier.From("default"),
                "Default Tenant"
            )
        );

        base.Configure(builder);
    }
}
