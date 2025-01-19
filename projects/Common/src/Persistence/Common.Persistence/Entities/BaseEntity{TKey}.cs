namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;

public class BaseEntity<TKey>(TKey id) : IBaseEntity
{
    public TKey Id { get; set; } = id;

    public DateTimeOffset? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastUpdatedAt { get; set; }

    public string? LastUpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }
}
