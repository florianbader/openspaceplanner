using FluentValidation;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Topics;

public class TopicValidator : AbstractValidator<TopicDto>
{
    public TopicValidator()
    {
        RuleFor(entity => entity.Name).NotEmpty().MaximumLength(DataTypes.ShortTextLength);
        RuleFor(entity => entity.Description).MaximumLength(DataTypes.LongTextLength);
    }
}
