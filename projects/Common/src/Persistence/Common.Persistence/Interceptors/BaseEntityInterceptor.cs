using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Interceptors;

public class BaseEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        _ = eventData.Context ?? throw new ArgumentException("Database context not found");

        UpdateTimestamps(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        _ = eventData.Context ?? throw new ArgumentException("Database context not found");

        UpdateTimestamps(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateTimestamps(DbContext context)
    {
        var entries = context.ChangeTracker.Entries<BaseEntity>();
        var utcNow = DateTimeOffset.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt).CurrentValue = utcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(p => p.LastUpdatedAt).CurrentValue = utcNow;
            }
            else if (entry.State == EntityState.Deleted && entry.Entity.IsDeleted)
            {
                // soft delete if it's not already soft deleted and we try to hard delete it
                entry.Property(p => p.DeletedAt).CurrentValue = utcNow;
                entry.Property(p => p.IsDeleted).CurrentValue = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}
