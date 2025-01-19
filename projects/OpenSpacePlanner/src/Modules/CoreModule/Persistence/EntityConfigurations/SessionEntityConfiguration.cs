using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.EntityConfigurations;

public class SessionEntityConfiguration : TenantBaseEntityConfiguration<Session, SessionId>
{
    public override void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.IsShortText(p => p.Name);
        builder.IsLongText(p => p.Description);

        builder.HasMany(p => p.Rooms).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(p => p.TimeSlots).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(p => p.Topics).WithOne().OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}
