using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.EntityConfigurations;

public class TopicEntityConfiguration : TenantBaseEntityConfiguration<Topic, TopicId>
{
    public override void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.IsShortText(p => p.Name);
        builder.IsLongText(p => p.Description);

        builder.HasOne(p => p.TimeSlot).WithMany();
        builder.HasOne(p => p.Room).WithMany();

        base.Configure(builder);
    }
}
