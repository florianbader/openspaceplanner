using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Rooms;

public static class CreateRoom
{
    /// <summary>
    /// The controller for handling the rooms.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : RoomsControllerBase
    {
        /// <summary>
        /// Creates a new rooms item.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="item">The new room.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The created room.</returns>
        [HttpPost]
        public Task<RoomDto> CreateAsync(
            SessionId sessionId,
            CreateRoomCommandDto item,
            CancellationToken cancellationToken
        ) => mediator.Send(new CreateRoomCommand(sessionId, item.Name), cancellationToken);
    }

    public class Handler(DbContext databaseContext, ILogger<Handler> logger)
        : IRequestHandler<CreateRoomCommand, RoomDto>
    {
        public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new Room(request.SessionId, request.Name);
            var entityEntry = databaseContext.Add(room);

            await databaseContext.SaveChangesAsync(cancellationToken);

            logger.LogTrace("Added entity with id {Id}", entityEntry.Entity.Id);

            var dto = RoomDto.From(entityEntry.Entity);
            return dto;
        }
    }

    public record CreateRoomCommand(SessionId SessionId, string Name) : IRequest<RoomDto>;

    public record CreateRoomCommandDto(string Name);

    public class Validator : AbstractValidator<CreateRoomCommand>
    {
        public Validator() => RuleFor(entity => entity.Name).NotEmpty().MaximumLength(DataTypes.ShortTextLength);
    }
}
