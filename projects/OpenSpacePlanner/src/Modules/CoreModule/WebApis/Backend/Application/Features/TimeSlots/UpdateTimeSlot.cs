using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Extensions;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.TimeSlots;

public static class UpdateTimeSlot
{
    /// <summary>
    /// The controller for handling the time slots.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : TimeSlotControllerBase
    {
        /// <summary>
        /// Updates the specific time slot.
        /// </summary>
        /// <param name="sessionId">The id of the session.</param>
        /// <param name="id">The id of the time slot.</param>
        /// <param name="item">The new time slot.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated time slot.</returns>
        [HttpPut("{id}")]
        public Task<TimeSlotDto> UpdateAsync(
            SessionId sessionId,
            TimeSlotId id,
            TimeSlotDto item,
            CancellationToken cancellationToken
        ) => mediator.Send(new UpdateTimeSlotCommand(sessionId, id, item), cancellationToken);
    }

    public class Handler(DbContext databaseContext) : IRequestHandler<UpdateTimeSlotCommand, TimeSlotDto>
    {
        public async Task<TimeSlotDto> Handle(UpdateTimeSlotCommand request, CancellationToken cancellationToken)
        {
            var entity = await databaseContext.Set<TimeSlot>().FindOrThrowAsync(request.Id, cancellationToken);
            entity.Name = request.TimeSlot.Name;

            await databaseContext.SaveChangesAsync(cancellationToken);

            var dto = TimeSlotDto.From(entity);
            return dto;
        }
    }

    public record UpdateTimeSlotCommand(SessionId SessionId, TimeSlotId Id, TimeSlotDto TimeSlot)
        : IRequest<TimeSlotDto>;

    public class Validator : AbstractValidator<UpdateTimeSlotCommand>
    {
        public Validator() => RuleFor(entity => entity.TimeSlot).SetValidator(new TimeSlotValidator());
    }
}
