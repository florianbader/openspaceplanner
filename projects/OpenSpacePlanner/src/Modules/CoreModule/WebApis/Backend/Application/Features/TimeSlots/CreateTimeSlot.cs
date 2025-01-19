using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.TimeSlots;

public static class CreateTimeSlot
{
    /// <summary>
    /// The controller for handling the time slots.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public class Controller(IMediator mediator) : TimeSlotControllerBase
    {
        /// <summary>
        /// Creates a new time slots item.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="item">The new time slot.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The created time slot.</returns>
        [HttpPost]
        public Task<TimeSlotDto> CreateAsync(
            SessionId sessionId,
            CreateTimeSlotCommandDto item,
            CancellationToken cancellationToken
        ) => mediator.Send(new CreateTimeSlotCommand(sessionId, item.Name), cancellationToken);
    }

    public class Handler(DbContext databaseContext, ILogger<Handler> logger)
        : IRequestHandler<CreateTimeSlotCommand, TimeSlotDto>
    {
        public async Task<TimeSlotDto> Handle(CreateTimeSlotCommand request, CancellationToken cancellationToken)
        {
            var entity = new TimeSlot(request.SessionId, request.Name);
            var entityEntry = databaseContext.Add(entity);

            await databaseContext.SaveChangesAsync(cancellationToken);

            logger.LogTrace("Added entity with id {Id}", entityEntry.Entity.Id);

            var dto = TimeSlotDto.From(entityEntry.Entity);
            return dto;
        }
    }

    public record CreateTimeSlotCommand(SessionId SessionId, string Name) : IRequest<TimeSlotDto>;

    public record CreateTimeSlotCommandDto(string Name);

    public class Validator : AbstractValidator<CreateTimeSlotCommand>
    {
        public Validator() => RuleFor(entity => entity.Name).NotEmpty().MaximumLength(DataTypes.ShortTextLength);
    }
}
