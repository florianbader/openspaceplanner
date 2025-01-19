using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Interceptors;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;

public static class EntityFramework
{
    public static void AddEntityFramework<TDatabaseContext>(
        this IServiceCollection services,
        IConfiguration configuration
    )
        where TDatabaseContext : DbContext
    {
        services.AddDbContext<TDatabaseContext>(configure =>
        {
            configure
                .UseSqlServer(configuration.GetConnectionString("database"))
                .AddInterceptors(new BaseEntityInterceptor());

            // track all entities when they are received from the database
            // make sure to track no changes if entities are only queried but not edited
            configure.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        });
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<TDatabaseContext>());
    }

    public static void MigrateDatabaseLocal<TDatabaseContext>(this IApplicationBuilder app)
        where TDatabaseContext : DbContext
    {
        try
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<TDatabaseContext>();
            applicationDbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Could not migrate database.", ex);
        }
    }
}
