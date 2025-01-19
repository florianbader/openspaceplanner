using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;

public static class ApiVersioning
{
    public static void AddCustomApiVersioning(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddEndpointsApiExplorer();

        serviceCollection
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;

                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader(), // ?api-version=1.0
                    new MediaTypeApiVersionReader()
                ); // application/json;v=1.0

                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                options.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }
}
