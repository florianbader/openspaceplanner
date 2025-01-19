using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Extensions;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Topics;

public static class UpdateTopic
{
    /// <summary>
    /// The controller for handling the topics.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : TopicsControllerBase
    {
        /// <summary>
        /// Updates the specific topic.
        /// </summary>
        /// <param name="sessionId">The id of the session.</param>
        /// <param name="id">The id of the topic.</param>
        /// <param name="item">The new topic.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated topic.</returns>
        [HttpPut("{id}")]
        public Task<TopicDto> UpdateAsync(
            SessionId sessionId,
            TopicId id,
            TopicDto item,
            CancellationToken cancellationToken
        ) => mediator.Send(new UpdateTopicCommand(sessionId, id, item), cancellationToken);
    }

    public class Handler(DbContext databaseContext) : IRequestHandler<UpdateTopicCommand, TopicDto>
    {
        public async Task<TopicDto> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
        {
            var entity = await databaseContext.Set<Topic>().FindOrThrowAsync(request.Id, cancellationToken);
            entity.Name = request.Topic.Name;

            await databaseContext.SaveChangesAsync(cancellationToken);

            var dto = TopicDto.From(entity);
            return dto;
        }
    }

    public record UpdateTopicCommand(SessionId SessionId, TopicId Id, TopicDto Topic) : IRequest<TopicDto>;

    public class Validator : AbstractValidator<UpdateTopicCommand>
    {
        public Validator() => RuleFor(entity => entity.Topic).SetValidator(new TopicValidator());
    }
}
