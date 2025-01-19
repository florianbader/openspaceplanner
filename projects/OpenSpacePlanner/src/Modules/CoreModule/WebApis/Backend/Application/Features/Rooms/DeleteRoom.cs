using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Extensions;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Rooms;

public static class DeleteRoom
{
    /// <summary>
    /// The controller for handling the rooms.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : RoomsControllerBase
    {
        /// <summary>
        /// Deletes the specific room.
        /// </summary>
        /// <param name="sessionId">The id of the session.</param>
        /// <param name="id">The id of the room.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The async task.</returns>
        [HttpDelete("{id}")]
        public Task DeleteAsync(SessionId sessionId, RoomId id, CancellationToken cancellationToken) =>
            mediator.Send(new Command(sessionId, id), cancellationToken);
    }

    public class Handler(DbContext databaseContext, ILogger<Handler> logger) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await databaseContext
                .Set<Room>()
                .TagWithCallSite(nameof(DeleteRoom))
                .Where(entity => entity.SessionId == request.SessionId && entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            logger.LogDebug("Deleted entity with id {Id}", request.Id);
        }
    }

    public record Command(SessionId SessionId, RoomId Id) : IRequest;
}
