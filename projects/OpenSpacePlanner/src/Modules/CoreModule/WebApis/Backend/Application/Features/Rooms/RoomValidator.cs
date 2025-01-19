using FluentValidation;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Rooms;

public class RoomValidator : AbstractValidator<RoomDto>
{
    public RoomValidator()
    {
        RuleFor(entity => entity.Name).NotEmpty().MaximumLength(DataTypes.ShortTextLength);
        RuleFor(entity => entity.Description).MaximumLength(DataTypes.LongTextLength);
    }
}
