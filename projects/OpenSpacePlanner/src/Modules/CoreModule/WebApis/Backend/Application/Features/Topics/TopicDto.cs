using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Topics;

public record TopicDto(Guid Id, string Name, string? Description, Guid? RoomId, Guid? TimeSlotId)
{
    public static TopicDto From(Topic entity) =>
        new(
            (Guid)entity.Id,
            entity.Name,
            entity.Description,
            entity.Room?.Id.Value,
            entity.TimeSlot?.Id.Value
        );
}
