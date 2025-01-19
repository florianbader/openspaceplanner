using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Extensions;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Controllers;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Sessions;

public static class DeleteSession
{
    /// <summary>
    /// The controller for handling the to do items.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Deletes the specific to do item.
        /// </summary>
        /// <param name="id">The id of the to do item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The async task.</returns>
        [HttpDelete("{id}")]
        public Task DeleteAsync(SessionId id, CancellationToken cancellationToken) =>
            mediator.Send(new Command(id), cancellationToken);
    }

    public class Handler(DbContext databaseContext, ILogger<Handler> logger) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await databaseContext
                .Set<Session>()
                .TagWithCallSite(nameof(DeleteSession))
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            logger.LogDebug("Deleted entity with id {Id}", request.Id);
        }
    }

    public record Command(SessionId Id) : IRequest;
}
