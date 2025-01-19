using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Behaviors;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Extensions;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Models;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;

public static class Mediator
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddTransientServicesFromAssembly(typeof(ValidationResult), typeof(IValidator<>));

        var assemblies = services.GetApplicationAssemblies();
        services.AddMediatR(options => options.RegisterServicesFromAssemblies(assemblies.ToArray()));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
    }
}
