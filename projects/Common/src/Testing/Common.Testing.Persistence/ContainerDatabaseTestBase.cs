using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;

namespace RioScaffolding.OpenSpacePlanner.Common.Testing.Persistence;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "IDisposableAnalyzers.Correctness",
    "IDISP026:Class with no virtual DisposeAsyncCore method should be sealed",
    Justification = "Not using IDisposable but IAsyncLifetime"
)]
public abstract class ContainerDatabaseTestBase<TDbContext>(DatabaseFixture databaseFixture) : TestBase, IAsyncLifetime
    where TDbContext : DbContext
{
    private Respawner? _respawner;
    private string? _connectionString;

    protected TDbContext? DatabaseContext { get; private set; }

    public virtual async ValueTask DisposeAsync()
    {
        _ =
            _connectionString
            ?? throw new InvalidOperationException("Could not find connection string in container database test base");

        if (_respawner is not null)
        {
            await _respawner.ResetAsync(_connectionString);
        }

        if (DatabaseContext is not null)
        {
            await DatabaseContext.DisposeAsync();
        }

        var connectionStringBuilder = new SqlConnectionStringBuilder(_connectionString);
        databaseFixture.ReleaseDatabase(connectionStringBuilder.InitialCatalog);

        GC.SuppressFinalize(this);
    }

    public virtual async ValueTask InitializeAsync()
    {
        _connectionString = await databaseFixture.GetDatabaseConnectionStringAsync(
            TestContext.Current.CancellationToken
        );

        var databaseContextOptionsBuilder = new DbContextOptionsBuilder<TDbContext>().UseSqlServer(_connectionString);

        DatabaseContext =
            Activator.CreateInstance(typeof(TDbContext), databaseContextOptionsBuilder.Options) as TDbContext
            ?? throw new InvalidOperationException("Could not create database context in container database test base");
        await DatabaseContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        _respawner = await Respawner.CreateAsync(
            _connectionString,
            new RespawnerOptions { DbAdapter = DbAdapter.SqlServer }
        );
    }
}
