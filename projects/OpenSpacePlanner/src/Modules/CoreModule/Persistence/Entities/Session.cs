using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;
using Vogen;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

public class Session(SessionId id, string name) : BaseEntity<SessionId>(id)
{
    public Session(string name)
        : this(SessionId.From(SequentialGuid.NewSequentialGuid()), name) { }

    public string Name { get; set; } = name;

    public string? Description { get; set; }

    public virtual ICollection<Room> Rooms { get; } = new HashSet<Room>();

    public virtual ICollection<TimeSlot> TimeSlots { get; } = new HashSet<TimeSlot>();

    public virtual ICollection<Topic> Topics { get; } = new HashSet<Topic>();
}

[ValueObject<Guid>]
public readonly partial struct SessionId;
