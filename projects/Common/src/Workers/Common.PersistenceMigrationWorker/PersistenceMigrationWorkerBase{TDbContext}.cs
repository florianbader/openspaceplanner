using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RioScaffolding.OpenSpacePlanner.Common.Workers;

public abstract class PersistenceMigrationWorkerBase<TDbContext>(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime
) : PersistenceMigrationWorkerBase
    where TDbContext : DbContext
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

            await EnsureDatabaseAsync(dbContext, stoppingToken);
            await RunMigrationAsync(dbContext, stoppingToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(TDbContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            if (await dbCreator.ExistsAsync(cancellationToken))
            {
                return;
            }

            await dbCreator.CreateAsync(cancellationToken);
        });
    }

    private static async Task RunMigrationAsync(TDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }
}
