using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RioScaffolding.OpenSpacePlanner.Common.Testing.Persistence;

namespace RioScaffolding.OpenSpacePlanner.Common.Testing.WebApi;

[Trait("Category", "WebApi")]
public abstract class WebApiTestBase<TProgram, TDbContext>
    : ContainerDatabaseTestBase<TDbContext>,
        IClassFixture<CustomWebApplicationFactory<TProgram>>,
        IDisposable
    where TProgram : class
    where TDbContext : DbContext
{
    private readonly IServiceScope _scope;
    private bool _isDisposed;
    private HttpClient? _client;

    protected WebApiTestBase(DatabaseFixture databaseFixture, CustomWebApplicationFactory<TProgram> factory)
        : base(databaseFixture)
    {
        Factory = factory;

        _scope = Factory.Services.CreateScope();
    }

    protected HttpClient Client => _client ??= Factory.CreateClient();

    protected CustomWebApplicationFactory<TProgram> Factory { get; }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _scope.Dispose();
            _client?.Dispose();
        }

        _isDisposed = true;
    }
}
