using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;

/// <inheritdoc />
/// <remarks>
/// This factory class is used to create a DbContext instance during design time, such as when running Entity Framework Core tools.
/// It retrieves the database connection string from the environment variable "DatabaseConnectionString" if not provided in the arguments.
/// </remarks>
public abstract class DbContextDesignTimeFactoryBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    /// <inheritdoc />
    public TDbContext CreateDbContext(string[] args)
    {
        var databaseConnectionString =
            args.Length > 0 ? args[0] : Environment.GetEnvironmentVariable("DatabaseConnectionString");

        var optionsBuilder = new DbContextOptionsBuilder<TDbContext>().UseSqlServer(databaseConnectionString);

        return Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options) as TDbContext
            ?? throw new InvalidOperationException("Could not create database context during design time");
    }
}
