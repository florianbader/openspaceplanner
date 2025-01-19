using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Sessions;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Tests.Features.Sessions;

public class CreateSessionTests : TestBase
{
    [Fact]
    public async Task CreateSession_WithTooLongName_ShouldNotBeAdded()
    {
        var validator = new CreateSession.Validator();
        var result = await validator.ValidateAsync(
            new CreateSession.CreateSessionCommand(Faker.Random.String2(DataTypes.ShortTextLength + 1)),
            TestContext.Current.CancellationToken
        );

        result.IsValid.Should().BeFalse();
    }
}
