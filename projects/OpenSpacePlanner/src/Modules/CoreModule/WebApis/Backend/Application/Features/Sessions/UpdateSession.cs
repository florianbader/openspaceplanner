using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Controllers;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Extensions;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Sessions;

public static class UpdateSession
{
    /// <summary>
    /// The controller for handling the sessions.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Updates the specific session.
        /// </summary>
        /// <param name="id">The id of the session.</param>
        /// <param name="command">The updated session object.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated session.</returns>
        [HttpPut("{id}")]
        public Task<SessionDto> UpdateAsync(SessionId id, UpdateSession command, CancellationToken cancellationToken) =>
            mediator.Send(new UpdateSessionCommand(id, command.Name), cancellationToken);

        public record UpdateSession(string Name);
    }

    public class Handler(DbContext databaseContext) : IRequestHandler<UpdateSessionCommand, SessionDto>
    {
        public async Task<SessionDto> Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
        {
            var entity = await databaseContext.Set<Session>().FindOrThrowAsync(request.Id, cancellationToken);
            entity.Name = request.Name;

            await databaseContext.SaveChangesAsync(cancellationToken);

            var dto = SessionDto.From(entity);
            return dto;
        }
    }

    public record UpdateSessionCommand(SessionId Id, string Name) : IRequest<SessionDto>;

    public class Validator : AbstractValidator<UpdateSessionCommand>
    {
        public Validator() => RuleFor(entity => entity.Name).NotEmpty().MaximumLength(DataTypes.ShortTextLength);
    }
}
