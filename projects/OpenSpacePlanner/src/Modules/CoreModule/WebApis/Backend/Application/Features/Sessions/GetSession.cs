using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Extensions;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Controllers;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Exceptions;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Sessions;

public static class GetSession
{
    /// <summary>
    /// The controller for handling the sessions.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Gets the specific session.
        /// </summary>
        /// <param name="id">The id of the session.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The specific session.</returns>
        [HttpGet("{id}")]
        public Task<SessionDto> GetAsync(SessionId id, CancellationToken cancellationToken) =>
            mediator.Send(new Query(id), cancellationToken);
    }

    public class Handler(DbContext databaseContext) : IRequestHandler<Query, SessionDto>
    {
        public async Task<SessionDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity =
                await databaseContext
                    .Set<Session>()
                    .AsQuery(nameof(GetSession))
                    .Include(entity => entity.Rooms)
                    .Include(entity => entity.TimeSlots)
                    .Include(entity => entity.Topics)
                    .FirstOrDefaultAsync(entity => entity.Id == request.Id, cancellationToken)
                ?? throw new EntityNotFoundException();

            var dto = SessionDto.From(entity);
            return dto;
        }
    }

    public record Query(SessionId Id) : IRequest<SessionDto>;
}
