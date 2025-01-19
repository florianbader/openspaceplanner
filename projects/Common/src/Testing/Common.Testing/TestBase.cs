using RioScaffolding.OpenSpacePlanner.Common.Testing.Extensions;

namespace RioScaffolding.OpenSpacePlanner.Common.Testing;

public abstract class TestBase
{
    protected TestBase()
    {
        Fixture = new Fixture().AddDefaults();
        Faker = new Faker();
    }

    public Faker Faker { get; }

    public IFixture Fixture { get; }
}
