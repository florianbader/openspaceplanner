using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.Common.Testing.Builders;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Tests.Builders;

internal sealed class SessionBuilderConfiguration : ITestObjectBuilderConfiguration<Session>
{
    public Faker<Session> Configure(Faker<Session> faker) =>
        faker
            .RuleFor(p => p.Id, (fake, _) => SessionId.From(fake.Random.Guid()))
            .RuleFor(p => p.Name, fake => fake.Random.String2(DataTypes.ShortTextLength));
}
