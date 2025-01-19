using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;

public static class FileSystem
{
    public static void AddFileSystem(this IServiceCollection serviceCollection) =>
        serviceCollection.AddSingleton<IFileSystem, System.IO.Abstractions.FileSystem>();
}
