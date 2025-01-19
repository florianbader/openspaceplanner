using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RioScaffolding.OpenSpacePlanner.Common.Extensions;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddTransientServicesFromAssembly(
        this IServiceCollection services,
        Type assemblyType,
        Type interfaceType
    )
    {
        var types = interfaceType.IsGenericType
            ? assemblyType
                .Assembly.GetTypes()
                .Where(t => GetFirstMatchingGenericInterfaceType(t, interfaceType) is not null)
            : assemblyType.Assembly.GetTypes().Where(interfaceType.IsAssignableFrom);

        foreach (var type in types)
        {
            var matchingInterface = interfaceType.IsGenericType
                ? GetFirstMatchingGenericInterfaceType(type, interfaceType)
                : interfaceType;

            if (matchingInterface is not null)
            {
                services.AddTransient(matchingInterface, type);
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "Used for extension method"
    )]
    public static IEnumerable<Assembly> GetApplicationAssemblies(this IServiceCollection services)
    {
        var assemblyPrefix = Assembly.GetExecutingAssembly().GetProjectNamePrefix();

        var assemblies = AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(assembly =>
                assembly.FullName is not null && assembly.FullName.StartsWith(assemblyPrefix, StringComparison.Ordinal)
            )
            .ToArray();
        return assemblies;
    }

    private static Type? GetFirstMatchingGenericInterfaceType(Type type, Type interfaceType) =>
        Array.Find(type.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);
}
