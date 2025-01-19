namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;

public interface IBaseEntity
{
    DateTimeOffset? CreatedAt { get; set; }

    string? CreatedBy { get; set; }

    DateTimeOffset? LastUpdatedAt { get; set; }

    string? LastUpdatedBy { get; set; }

    bool IsDeleted { get; set; }

    DateTimeOffset? DeletedAt { get; set; }

    string? DeletedBy { get; set; }
}
