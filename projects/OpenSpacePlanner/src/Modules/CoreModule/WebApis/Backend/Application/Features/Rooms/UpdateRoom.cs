using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Extensions;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Rooms;

public static class UpdateRoom
{
    /// <summary>
    /// The controller for handling the rooms.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : RoomsControllerBase
    {
        /// <summary>
        /// Updates the specific room.
        /// </summary>
        /// <param name="sessionId">The id of the session.</param>
        /// <param name="id">The id of the room.</param>
        /// <param name="item">The new room.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated room.</returns>
        [HttpPut("{id}")]
        public Task<RoomDto> UpdateAsync(
            SessionId sessionId,
            RoomId id,
            RoomDto item,
            CancellationToken cancellationToken
        ) => mediator.Send(new UpdateRoomCommand(sessionId, id, item), cancellationToken);
    }

    public class Handler(DbContext databaseContext) : IRequestHandler<UpdateRoomCommand, RoomDto>
    {
        public async Task<RoomDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var entity = await databaseContext.Set<Room>().FindOrThrowAsync(request.Id, cancellationToken);
            entity.Name = request.Room.Name;

            await databaseContext.SaveChangesAsync(cancellationToken);

            var dto = RoomDto.From(entity);
            return dto;
        }
    }

    public record UpdateRoomCommand(SessionId SessionId, RoomId Id, RoomDto Room) : IRequest<RoomDto>;

    public class Validator : AbstractValidator<UpdateRoomCommand>
    {
        public Validator() => RuleFor(entity => entity.Room).SetValidator(new RoomValidator());
    }
}
