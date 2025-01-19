using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Controllers;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Sessions;

public static class CreateSession
{
    /// <summary>
    /// The controller for handling the sessions.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Creates a new sessions item.
        /// </summary>
        /// <param name="item">The new session.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The created session.</returns>
        [HttpPost]
        public Task<SessionDto> CreateAsync(CreateSessionCommand item, CancellationToken cancellationToken) =>
            mediator.Send(item, cancellationToken);
    }

    public class Handler(DbContext databaseContext, ILogger<Handler> logger)
        : IRequestHandler<CreateSessionCommand, SessionDto>
    {
        public async Task<SessionDto> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            var session = new Session(request.Name);
            var entityEntry = databaseContext.Add(session);

            await databaseContext.SaveChangesAsync(cancellationToken);

            logger.LogTrace("Added entity with id {Id}", entityEntry.Entity.Id);

            var dto = SessionDto.From(entityEntry.Entity);
            return dto;
        }
    }

    public record CreateSessionCommand(string Name) : IRequest<SessionDto>;

    public class Validator : AbstractValidator<CreateSessionCommand>
    {
        public Validator() => RuleFor(entity => entity.Name).NotEmpty().MaximumLength(DataTypes.ShortTextLength);
    }
}
