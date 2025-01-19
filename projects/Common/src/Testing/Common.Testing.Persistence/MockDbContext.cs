using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MockQueryable.NSubstitute;

namespace RioScaffolding.OpenSpacePlanner.Common.Testing.Persistence;

public class MockDbContext : DbContext
{
    private readonly ConcurrentDictionary<Type, List<object>> _sets = new();

    public int SavedCount { get; private set; }

    public override DbSet<TEntity> Set<TEntity>()
    {
        var set = _sets.GetOrAdd(typeof(TEntity), _ => []).OfType<TEntity>().ToList();
        return set.BuildMockDbSet();
    }

    public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        where TEntity : class
    {
        var set = _sets.GetOrAdd(typeof(TEntity), _ => []);
        set.Add(entity);

        var entityEntry = CreateEntityEntry(entity);
        entityEntry.State = EntityState.Added;
        return entityEntry;
    }

    public override EntityEntry Add(object entity)
    {
        var set = _sets.GetOrAdd(entity.GetType(), _ => []);
        set.Add(entity);

        var entityEntry = CreateEntityEntry(entity);
        entityEntry.State = EntityState.Added;
        return entityEntry;
    }

    public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(Add(entity));

    public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken = default
    ) => ValueTask.FromResult(Add(entity));

    public override void AddRange(IEnumerable<object> entities)
    {
        if (!entities.Any())
        {
            return;
        }

        var set = _sets.GetOrAdd(entities.First().GetType(), _ => []);
        set.AddRange(entities);
    }

    public override void AddRange(params object[] entities) => AddRange(entities.AsEnumerable());

    public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
    {
        AddRange(entities);
        return Task.CompletedTask;
    }

    public override Task AddRangeAsync(params object[] entities)
    {
        AddRange(entities);
        return Task.CompletedTask;
    }

    public override EntityEntry Attach(object entity) => CreateEntityEntry(entity);

    public override EntityEntry<TEntity> Attach<TEntity>(TEntity entity) => CreateEntityEntry(entity);

    public override void AttachRange(IEnumerable<object> entities)
    {
        // do nothing
    }

    public override void AttachRange(params object[] entities)
    {
        // do nothing
    }

    public override EntityEntry Entry(object entity) => CreateEntityEntry(entity);

    public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity) => CreateEntityEntry(entity);

    public override int SaveChanges() => SavedCount++;

    public override int SaveChanges(bool acceptAllChangesOnSuccess) => SavedCount++;

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    ) => Task.FromResult(SaveChanges(acceptAllChangesOnSuccess));

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(SaveChanges());

    public override EntityEntry Remove(object entity)
    {
        var set = _sets.GetOrAdd(entity.GetType(), _ => []);
        set.Remove(entity);

        var entityEntry = CreateEntityEntry(entity);
        entityEntry.State = EntityState.Deleted;
        return entityEntry;
    }

    public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
    {
        var set = _sets.GetOrAdd(typeof(TEntity), _ => []);
        set.Remove(entity);

        var entityEntry = CreateEntityEntry(entity);
        entityEntry.State = EntityState.Deleted;
        return entityEntry;
    }

    public override void RemoveRange(IEnumerable<object> entities)
    {
        if (!entities.Any())
        {
            return;
        }

        var set = _sets.GetOrAdd(entities.First().GetType(), _ => []);
        foreach (var entity in entities)
        {
            set.Remove(entity);
        }
    }

    public override void RemoveRange(params object[] entities) => RemoveRange(entities.AsEnumerable());

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "EF1001:Internal EF Core API usage.",
        Justification = "Used for testing"
    )]
    private static EntityEntry CreateEntityEntry(object entity) =>
        new(new InternalEntityEntry(Substitute.For<IStateManager>(), Substitute.For<IRuntimeEntityType>(), entity));

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "EF1001:Internal EF Core API usage.",
        Justification = "Used for testing"
    )]
    private static EntityEntry<TEntity> CreateEntityEntry<TEntity>(TEntity entity)
        where TEntity : class =>
        new(new InternalEntityEntry(Substitute.For<IStateManager>(), Substitute.For<IRuntimeEntityType>(), entity));
}
