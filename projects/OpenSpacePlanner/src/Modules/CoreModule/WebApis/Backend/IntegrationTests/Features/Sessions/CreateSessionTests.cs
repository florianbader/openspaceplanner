using FluentAssertions.Execution;
using RioScaffolding.OpenSpacePlanner.Common.Testing.Persistence;
using RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Sessions;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.IntegrationTests.Features.Sessions;

public class CreateSessionTests : MockDatabaseTestBase
{
    [Fact]
    public async Task CreateSession_WithCorrectName_ShouldAddANewTodoItem()
    {
        var expectedName = Faker.Random.String2(32);
        var request = new CreateSession.CreateSessionCommand(expectedName);

        var handler = Fixture.Create<CreateSession.Handler>();
        var dto = await handler.Handle(request, TestContext.Current.CancellationToken);

        using var _ = new AssertionScope();

        dto.Id.Should().NotBeEmpty();
        dto.Name.Should().Be(expectedName);

        DatabaseContextMock.SavedCount.Should().Be(1);
    }
}
