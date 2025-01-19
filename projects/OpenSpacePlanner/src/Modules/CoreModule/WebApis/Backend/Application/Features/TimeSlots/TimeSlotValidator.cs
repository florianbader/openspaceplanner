using FluentValidation;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.TimeSlots;

public class TimeSlotValidator : AbstractValidator<TimeSlotDto>
{
    public TimeSlotValidator()
    {
        RuleFor(entity => entity.Name).NotEmpty().MaximumLength(DataTypes.ShortTextLength);
        RuleFor(entity => entity.Description).MaximumLength(DataTypes.LongTextLength);
    }
}
