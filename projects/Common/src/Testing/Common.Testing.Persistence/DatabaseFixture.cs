using System.Collections.Concurrent;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace RioScaffolding.OpenSpacePlanner.Common.Testing.Persistence;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private static readonly int MaxDatabases = Environment.ProcessorCount;
    private readonly BlockingCollection<string> _availableDatabases = new(new ConcurrentQueue<string>(), MaxDatabases);

    public MsSqlContainer Container { get; } =
        new MsSqlBuilder()
            .WithPassword("My@SecretPassword!") // doesn't matter as it's just executed locally
            .WithReuse(true)
            .Build();

    public async ValueTask InitializeAsync()
    {
        await Container.StartAsync(TestContext.Current.CancellationToken);

        for (var i = 0; i < MaxDatabases; i++)
        {
            _availableDatabases.Add(Guid.NewGuid().ToString());
        }
    }

    public ValueTask DisposeAsync() => Container.DisposeAsync();

    public Task<string> GetDatabaseConnectionStringAsync(CancellationToken cancellationToken)
    {
        if (!_availableDatabases.TryTake(out var databaseName, Timeout.Infinite, cancellationToken))
        {
            databaseName = Guid.NewGuid().ToString();
        }

        var builder = new SqlConnectionStringBuilder(Container.GetConnectionString()) { InitialCatalog = databaseName };

        return Task.FromResult(builder.ConnectionString);
    }

    public void ReleaseDatabase(string databaseName) => _availableDatabases.Add(databaseName);
}
