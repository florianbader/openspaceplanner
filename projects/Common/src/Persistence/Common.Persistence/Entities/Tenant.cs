using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using Vogen;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;

public class Tenant(TenantId id, TenantIdentifier identifier, string name) : BaseEntity<TenantId>(id)
{
    public Tenant(TenantIdentifier identifier, string name)
        : this(TenantId.From(SequentialGuid.NewSequentialGuid()), identifier, name) { }

    public TenantIdentifier Identifier { get; set; } = identifier;

    public string Name { get; set; } = name;
}

[ValueObject<Guid>]
public readonly partial struct TenantId;

[ValueObject<string>]
public readonly partial struct TenantIdentifier;
