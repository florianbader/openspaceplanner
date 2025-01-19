using Hellang.Middleware.ProblemDetails;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.ProblemDetails;

public class ApplicationProblemDetails : StatusCodeProblemDetails
{
    public ApplicationProblemDetails(Exception exception, int statusCode)
        : base(statusCode) => Detail = exception.Message;
}
