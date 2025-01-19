namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Exceptions;

public class TenantNotFoundException : Exception
{
    public TenantNotFoundException() { }

    public TenantNotFoundException(string? message)
        : base(message) { }

    public TenantNotFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
