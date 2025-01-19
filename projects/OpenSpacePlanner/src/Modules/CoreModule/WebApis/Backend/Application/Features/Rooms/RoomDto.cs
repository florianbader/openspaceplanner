using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.Rooms;

public record RoomDto(Guid Id, string Name, string? Description)
{
    public static RoomDto From(Room entity) => new(entity.Id.Value, entity.Name, entity.Description);
}
