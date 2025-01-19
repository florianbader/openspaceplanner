using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vogen;

[assembly: VogenDefaults(
    conversions: Conversions.Default | Conversions.EfCoreValueConverter,
    staticAbstractsGeneration: StaticAbstractsGeneration.MostCommon
)]

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence;

public static class VogenExtensions
{
    public static void ApplyVogenEfConvertersFromAssembly(
        this ModelConfigurationBuilder configurationBuilder,
        Assembly assembly
    )
    {
        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            if (IsVogenValueObject(type) && TryGetEfValueConverter(type, out var efCoreConverterType))
            {
                configurationBuilder.Properties(type).HaveConversion(efCoreConverterType);
            }
        }
    }

    private static bool TryGetEfValueConverter(Type type, [NotNullWhen(true)] out Type? efCoreConverterType)
    {
        var inner = type.GetNestedTypes();

        foreach (var innerType in inner)
        {
            var isValueConverter = typeof(ValueConverter).IsAssignableFrom(innerType);
            var isEfCoreValueConverter = "EfCoreValueConverter".Equals(innerType.Name, StringComparison.Ordinal);

            if (!isValueConverter || !isEfCoreValueConverter)
            {
                continue;
            }

            efCoreConverterType = innerType;
            return true;
        }

        efCoreConverterType = null;
        return false;
    }

    private static bool IsVogenValueObject(MemberInfo targetType)
    {
        var generatedCodeAttribute = targetType.GetCustomAttribute<GeneratedCodeAttribute>();
        return "Vogen".Equals(generatedCodeAttribute?.Tool, StringComparison.Ordinal);
    }
}
