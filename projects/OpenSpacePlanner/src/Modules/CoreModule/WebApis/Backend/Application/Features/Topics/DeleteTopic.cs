using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Extensions;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Topics;

public static class DeleteTopic
{
    /// <summary>
    /// The controller for handling the to do items.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : TopicsControllerBase
    {
        /// <summary>
        /// Deletes the specific to do item.
        /// </summary>
        /// <param name="sessionId">The id of the session.</param>
        /// <param name="id">The id of the to do item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The async task.</returns>
        [HttpDelete("{id}")]
        public Task DeleteAsync(SessionId sessionId, TopicId id, CancellationToken cancellationToken) =>
            mediator.Send(new Command(sessionId, id), cancellationToken);
    }

    public class Handler(DbContext databaseContext, ILogger<Handler> logger) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await databaseContext
                .Set<Topic>()
                .TagWithCallSite(nameof(DeleteTopic))
                .Where(entity => entity.SessionId == request.SessionId && entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            logger.LogDebug("Deleted entity with id {Id}", request.Id);
        }
    }

    public record Command(SessionId SessionId, TopicId Id) : IRequest;
}
