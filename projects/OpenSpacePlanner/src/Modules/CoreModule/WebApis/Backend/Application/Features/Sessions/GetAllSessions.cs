using Gridify;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Extensions;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Models;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Controllers;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Sessions;

public static class GetAllSessions
{
    /// <summary>
    /// The controller for handling the sessions.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Gets all sessions.
        /// </summary>
        /// <param name="pageRequest">The page request object for pagination.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A paginated set of the sessions.</returns>
        [HttpGet]
        public Task<ItemsResult<SessionDto>> GetAllAsync(
            [FromQuery] GridifyQuery pageRequest,
            CancellationToken cancellationToken
        ) => mediator.Send(new Query(pageRequest), cancellationToken);
    }

    public class Handler(DbContext databaseContext) : IRequestHandler<Query, ItemsResult<SessionDto>>
    {
        public Task<ItemsResult<SessionDto>> Handle(Query request, CancellationToken cancellationToken) =>
            databaseContext
                .Set<Session>()
                .AsQuery(nameof(GetAllSessions))
                .GetItemsResultAsync(request.PageRequest, item => SessionDto.From(item), cancellationToken);
    }

    public record Query(GridifyQuery? PageRequest) : IRequest<ItemsResult<SessionDto>>;
}
