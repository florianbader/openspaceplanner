using System.Net;
using Hellang.Middleware.ProblemDetails;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Exceptions;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Models;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.ProblemDetails;

public class ErrorsProblemDetails : StatusCodeProblemDetails
{
    public ErrorsProblemDetails(InvalidInputException exception)
        : base((int)HttpStatusCode.BadRequest)
    {
        Detail = exception.Message;
        Errors = exception.Errors;
    }

    public IEnumerable<ValidationResult> Errors { get; set; }
}
