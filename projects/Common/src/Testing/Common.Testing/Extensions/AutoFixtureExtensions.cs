using AutoFixture.AutoNSubstitute;

namespace RioScaffolding.OpenSpacePlanner.Common.Testing.Extensions;

public static class AutoFixtureExtensions
{
    public static IFixture AddDefaults(this IFixture fixture)
    {
        // Automatically configures AutoNSubstitute to inject mocks for all interfaces and abstract classes.
        // Makes sure all members of the mocked objects are configured by default to return a value from the fixture.
        fixture.Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });

        // Ignore cyclic references when creating objects.
        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        return fixture;
    }
}
