using FluentValidation;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Sessions;

public class SessionValidator : AbstractValidator<SessionDto>
{
    public SessionValidator() => RuleFor(entity => entity.Name).NotEmpty().MaximumLength(DataTypes.ShortTextLength);
}
