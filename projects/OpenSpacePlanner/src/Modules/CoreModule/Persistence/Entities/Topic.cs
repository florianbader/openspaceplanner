using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;
using Vogen;

namespace RioScaffolding.OpenSpacePlanner.CoreModule.Persistence.Entities;

public class Topic(TopicId id, SessionId sessionId, string name) : BaseEntity<TopicId>(id)
{
    public Topic(SessionId sessionId, string name)
        : this(TopicId.From(SequentialGuid.NewSequentialGuid()), sessionId, name) { }

    public string Name { get; set; } = name;

    public string? Description { get; set; }

    public virtual Room? Room { get; set; }

    public virtual TimeSlot? TimeSlot { get; set; }

    public SessionId SessionId { get; set; } = sessionId;
}

[ValueObject<Guid>]
public readonly partial struct TopicId;
