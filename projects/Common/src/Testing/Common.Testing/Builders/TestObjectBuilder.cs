using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace RioScaffolding.OpenSpacePlanner.Common.Testing.Builders;

public class TestObjectBuilder<TObject>
    where TObject : class
{
    private static readonly ConcurrentDictionary<Type, ITestObjectBuilderConfiguration<TObject>?> ConfigurationsCache =
        new();

    public TestObjectBuilder()
    {
        Faker = new Faker<TObject>();
        ConfigureFaker();
    }

    public Faker<TObject> Faker { get; }

    public TObject Generate() =>
        Faker
            .CustomInstantiator(_ =>
            {
                // Create an uninitialized instance of TEntity and populate it with fake data
                // We are doing this to generate a test instance of TEntity without invoking its constructor,
                // allowing us to populate it with controlled fake data for testing purposes.
                var obj = (TObject)RuntimeHelpers.GetUninitializedObject(typeof(TObject));
                Faker.Populate(obj);
                return obj;
            })
            .Generate();

    private static ITestObjectBuilderConfiguration<TObject>? GetConfiguration()
    {
        var type = typeof(TObject);
        if (ConfigurationsCache.TryGetValue(type, out var configuration))
        {
            return configuration;
        }

        var configurationType = AppDomain
            .CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t =>
                t.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(ITestObjectBuilderConfiguration<>)
                        && i.GetGenericArguments()[0] == type
                    )
            );

        if (configurationType != null)
        {
            configuration = Activator.CreateInstance(configurationType) as ITestObjectBuilderConfiguration<TObject>;
            ConfigurationsCache[type] = configuration;
        }

        return configuration;
    }

    private void ConfigureFaker()
    {
        var configuration = GetConfiguration();
        configuration?.Configure(Faker);
    }
}
