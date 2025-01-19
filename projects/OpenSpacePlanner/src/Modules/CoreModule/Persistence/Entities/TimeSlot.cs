using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;
using Vogen;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

public class TimeSlot(TimeSlotId id, SessionId sessionId, string name) : BaseEntity<TimeSlotId>(id)
{
    public TimeSlot(SessionId sessionId, string name)
        : this(TimeSlotId.From(SequentialGuid.NewSequentialGuid()), sessionId, name) { }

    public string Name { get; set; } = name;

    public string? Description { get; set; }

    public TimeOnly? FromTime { get; set; }

    public TimeOnly? ToTime { get; set; }

    public SessionId SessionId { get; set; } = sessionId;
}

[ValueObject<Guid>]
public readonly partial struct TimeSlotId;
