using RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Rooms;
using RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.TimeSlots;
using RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Topics;
using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Sessions;

public record SessionDto(
    Guid Id,
    string Name,
    IEnumerable<RoomDto> Rooms,
    IEnumerable<TimeSlotDto> TimeSlots,
    IEnumerable<TopicDto> Topics
)
{
    public static SessionDto From(Session entity) =>
        new(
            entity.Id.Value,
            entity.Name,
            entity.Rooms.Select(RoomDto.From),
            entity.TimeSlots.Select(TimeSlotDto.From),
            entity.Topics.Select(TopicDto.From)
        );
}
