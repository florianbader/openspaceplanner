using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.EntityConfigurations;

public class RoomEntityConfiguration : TenantBaseEntityConfiguration<Room, RoomId>
{
    public override void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.IsShortText(p => p.Name);
        builder.IsLongText(p => p.Description);

        base.Configure(builder);
    }
}
