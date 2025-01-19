using RioScaffolding.OpenSpacePlanner.Common.Testing.Persistence;
using RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Sessions;
using RioScaffolding.OpenSpacePlanner.Persistence;

[assembly: AssemblyFixture(typeof(DatabaseFixture))]
[assembly: AssemblyFixture(typeof(CustomWebApplicationFactory<Program>))]

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.IntegrationTests.Features.Sessions;

public class CreateSessionApiTests(DatabaseFixture databaseFixture, CustomWebApplicationFactory<Program> factory)
    : WebApiTestBase<Program, TenantApplicationDbContext>(databaseFixture, factory)
{
    [Fact]
    public async Task CreateSession_WithCorrectName_ShouldAdd()
    {
        const string expectedName = "test123";

        var request = new CreateSession.CreateSessionCommand(expectedName);
        using var response = await Client.PostAsJsonAsync("/default/api/sessions", request);

        response
            .Should()
            .Be200Ok()
            .And.Satisfy<SessionDto>(item =>
            {
                item.Id.Should().NotBeEmpty();
                item.Name.Should().Be(expectedName);
            });
    }
}
