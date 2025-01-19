using Microsoft.Extensions.DependencyInjection;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.Extensions;

public static class SignalRStartupExtensions
{
    public static IServiceCollection AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }
}
