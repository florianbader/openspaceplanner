using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Exceptions;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.ProblemDetails;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;

public static class ProblemDetails
{
    public static void AddProblemDetailsMapping(this IServiceCollection services) =>
        services.AddProblemDetails(options =>
        {
            options.Map<EntityNotFoundException>(ex => new ApplicationProblemDetails(
                ex,
                StatusCodes.Status404NotFound
            ));
            options.Map<InvalidInputException>(ex => new ErrorsProblemDetails(ex));
            options.Map<Exception>(ex => new ApplicationProblemDetails(ex, StatusCodes.Status500InternalServerError));
        });
}
