using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.EntityConfigurations;

public class TimeSlotEntityConfiguration : TenantBaseEntityConfiguration<TimeSlot, TimeSlotId>
{
    public override void Configure(EntityTypeBuilder<TimeSlot> builder)
    {
        builder.IsShortText(p => p.Name);
        builder.IsLongText(p => p.Description);

        base.Configure(builder);
    }
}
