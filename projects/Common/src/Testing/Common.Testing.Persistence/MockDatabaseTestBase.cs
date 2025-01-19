using Microsoft.EntityFrameworkCore;

namespace RioScaffolding.OpenSpacePlanner.Common.Testing.Persistence;

public abstract class MockDatabaseTestBase : TestBase
{
    public MockDbContext DatabaseContextMock { get; } = new();

    protected MockDatabaseTestBase()
    {
        Fixture.Register(() => DatabaseContextMock);
        Fixture.Register<DbContext>(() => DatabaseContextMock);
    }
}
