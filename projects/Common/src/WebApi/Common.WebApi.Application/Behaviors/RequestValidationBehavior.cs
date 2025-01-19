using FluentValidation;
using MediatR;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Exceptions;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Models;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Behaviors;

public class RequestValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var context = new ValidationContext<TRequest>(request);

        var tasks = _validators.Distinct().Select(v => v.ValidateAsync(context, cancellationToken));

        var result = await Task.WhenAll(tasks);

        var failures = result.SelectMany(result => result.Errors).Where(f => f != null).Distinct().ToArray();

        if (failures.Length != 0)
        {
            throw new InvalidInputException(failures.Select(f => new ValidationResult(f.ErrorCode, f.ErrorMessage)));
        }

        return await next();
    }
}
