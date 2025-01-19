using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;
using Vogen;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

public class Room(RoomId id, SessionId sessionId, string name) : BaseEntity<RoomId>(id)
{
    public Room(SessionId sessionId, string name)
        : this(RoomId.From(SequentialGuid.NewSequentialGuid()), sessionId, name) { }

    public SessionId SessionId { get; set; } = sessionId;

    public string Name { get; set; } = name;

    public string? Description { get; set; }
}

[ValueObject<Guid>]
public readonly partial struct RoomId;
