using System.Reflection;

namespace RioScaffolding.OpenSpacePlanner.Common.Extensions;

public static class AssemblyExtensions
{
    public static string GetAssemblyNamePrefix(this Assembly assembly, string prefix)
    {
        var currentAssemblyName =
            assembly.FullName ?? throw new InvalidOperationException("Could not get the current assembly name.");
        var indexOfPrefix = currentAssemblyName.IndexOf(prefix, StringComparison.Ordinal) - 1;
        if (indexOfPrefix < 0)
        {
            throw new InvalidOperationException("Could not find prefix");
        }

        var assemblyPrefix = currentAssemblyName[..indexOfPrefix];
        return assemblyPrefix;
    }

    public static IEnumerable<string> SplitAssemblyName(this Assembly assembly)
    {
        var currentAssemblyName =
            assembly.FullName ?? throw new InvalidOperationException("Could not get the current assembly name.");
        return currentAssemblyName.Split('.');
    }

    public static string GetProjectNamePrefix(this Assembly assembly) =>
        string.Join('.', assembly.SplitAssemblyName().Take(2));
}
