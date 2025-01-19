using System.Reflection;
using Gridify;
using Microsoft.Extensions.DependencyInjection;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Extensions;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;

public static class Gridify
{
    public static void AddGridify(this IServiceCollection services)
    {
        GridifyGlobalConfiguration.EnableEntityFrameworkCompatibilityLayer();

        services.ConfigureMappers();
    }

    private static void ConfigureMappers(this IServiceCollection services)
    {
        var assemblies = services.GetApplicationAssemblies();
        foreach (var assembly in assemblies)
        {
            services.ConfigureMappers(assembly);
        }
    }

    private static void ConfigureMappers(this IServiceCollection services, Assembly assembly)
    {
        var configurationGenericType = typeof(IGridifyConfiguration<>);

        var configurationTypes = assembly
            .GetTypes()
            .Where(type =>
                type is { IsAbstract: false, IsGenericTypeDefinition: false }
                && Array.Exists(
                    type.GetInterfaces(),
                    i => i.IsGenericType && i.GetGenericTypeDefinition() == configurationGenericType
                )
            );

        foreach (var configurationType in configurationTypes)
        {
            var configurationInterfaceType = configurationType
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == configurationGenericType);
            var genericArguments = configurationInterfaceType.GetGenericArguments();
            if (genericArguments is not { Length: 1 })
            {
                continue;
            }

            var targetType = genericArguments[0];

            var mapperType = typeof(GridifyMapper<>).MakeGenericType(targetType);
            var mapper = CreateMapperFromType(mapperType);

            ConfigureMapper(configurationType, mapperType, mapper);

            var interfaceType = typeof(IGridifyMapper<>).MakeGenericType(targetType);
            services.Add(new ServiceDescriptor(interfaceType, (_) => mapper, ServiceLifetime.Singleton));
        }
    }

    private static void ConfigureMapper(Type configurationType, Type mapperType, object mapper)
    {
        var configuration =
            Activator.CreateInstance(configurationType)
            ?? throw new InvalidOperationException("Could not create gridify configuration");

        configurationType
            .GetMethod("Configure", BindingFlags.Instance | BindingFlags.Public, [mapperType])
            ?.Invoke(configuration, [mapper]);
    }

    private static object CreateMapperFromType(Type mapperType)
    {
        var mapper =
            Activator.CreateInstance(mapperType, true, (ushort)0)
            ?? throw new InvalidOperationException("Could not create gridify mapper");

        mapperType
            .GetMethod("RemoveMap", BindingFlags.Instance | BindingFlags.Public, [typeof(string)])
            ?.Invoke(mapper, ["tenantId"]);
        mapperType
            .GetMethod("RemoveMap", BindingFlags.Instance | BindingFlags.Public, [typeof(string)])
            ?.Invoke(mapper, ["isDeleted"]);
        return mapper;
    }
}
