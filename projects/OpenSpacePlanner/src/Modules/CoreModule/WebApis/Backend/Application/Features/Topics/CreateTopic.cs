using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Topics;

public static class CreateTopic
{
    /// <summary>
    /// The controller for handling the topics.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : TopicsControllerBase
    {
        /// <summary>
        /// Creates a new topics item.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="item">The new topic.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The created topic.</returns>
        [HttpPost]
        public Task<TopicDto> CreateAsync(
            SessionId sessionId,
            CreateTopicCommandDto item,
            CancellationToken cancellationToken
        ) => mediator.Send(new CreateTopicCommand(sessionId, item.Name), cancellationToken);
    }

    public class Handler(DbContext databaseContext, ILogger<Handler> logger)
        : IRequestHandler<CreateTopicCommand, TopicDto>
    {
        public async Task<TopicDto> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = new Topic(request.SessionId, request.Name);
            var entityEntry = databaseContext.Add(topic);

            await databaseContext.SaveChangesAsync(cancellationToken);

            logger.LogTrace("Added entity with id {Id}", entityEntry.Entity.Id);

            var dto = TopicDto.From(entityEntry.Entity);
            return dto;
        }
    }

    public record CreateTopicCommand(SessionId SessionId, string Name) : IRequest<TopicDto>;

    public record CreateTopicCommandDto(string Name);

    public class Validator : AbstractValidator<CreateTopicCommand>
    {
        public Validator() => RuleFor(entity => entity.Name).NotEmpty().MaximumLength(DataTypes.ShortTextLength);
    }
}
