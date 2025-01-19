using RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Backend.WebApi.Application.Features.TimeSlots;

public record TimeSlotDto(Guid Id, string Name, string? Description, TimeOnly? FromTime, TimeOnly? ToTime)
{
    public static TimeSlotDto From(TimeSlot entity) =>
        new(entity.Id.Value, entity.Name, entity.Description, entity.FromTime, entity.ToTime);
}
