using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.Extensions;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;

/// <summary>
/// Represents the application's database context, providing access to the database and configuring entity models.
/// </summary>
/// <param name="options">The options to be used by the DbContext.</param>
public abstract class ApplicationDbContextBase(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ApplyConfigurationsFromAssemblies(modelBuilder);
    }

    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        foreach (var assembly in CachedAssemblies.Value)
        {
            configurationBuilder.ApplyVogenEfConvertersFromAssembly(assembly);
        }
    }

    private static void ApplyConfigurationsFromAssemblies(ModelBuilder modelBuilder)
    {
        foreach (var assembly in CachedAssemblies.Value)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }

    private static readonly Lazy<Assembly[]> CachedAssemblies = new(() =>
    {
        var assemblyPrefix = Assembly.GetExecutingAssembly().GetProjectNamePrefix();

        // make sure our assemblies are all loaded so we can find all entity configurations
        LoadEntityConfigurationAssemblies();

        var assemblies = AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(assembly =>
                assembly.FullName?.StartsWith(assemblyPrefix, StringComparison.Ordinal) == true
                && assembly.FullName?.Contains("Persistence", StringComparison.Ordinal) == true
            )
            .ToArray();
        return assemblies;
    });

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Major Code Smell",
        "S3885:\"Assembly.Load\" should be used",
        Justification = "Load exactly the assembly from disk"
    )]
    private static void LoadEntityConfigurationAssemblies()
    {
        var persistenceDlls = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.Persistence.dll");
        foreach (var persistenceDll in persistenceDlls)
        {
            var assemblyName = AssemblyName.GetAssemblyName(persistenceDll);
            var assemblyLoaded = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName == assemblyName.FullName);
            if (!assemblyLoaded)
            {
                Assembly.LoadFrom(persistenceDll);
            }
        }
    }
}
