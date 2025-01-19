using System.Globalization;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Entities;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence;

public class EntityFrameworkTenantStore<TDbContext>(TDbContext dbContext) : IMultiTenantStore<TenantInfo>
    where TDbContext : DbContext
{
    public virtual Task<TenantInfo?> TryGetAsync(string id)
    {
        var tenantId = TenantId.Parse(id, CultureInfo.InvariantCulture);
        return dbContext
            .Set<Tenant>()
            .AsNoTracking()
            .Where(ti => ti.Id == tenantId)
            .Select(ti => new TenantInfo
            {
                Id = ti.Id.ToString(),
                Name = ti.Name,
                Identifier = (string)ti.Identifier,
            })
            .SingleOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<TenantInfo>> GetAllAsync() =>
        await dbContext
            .Set<Tenant>()
            .AsNoTracking()
            .Select(ti => new TenantInfo
            {
                Id = ti.Id.ToString(),
                Name = ti.Name,
                Identifier = (string)ti.Identifier,
            })
            .ToListAsync();

    public virtual Task<TenantInfo?> TryGetByIdentifierAsync(string identifier)
    {
        var tenantIdentifier = TenantIdentifier.Parse(identifier, CultureInfo.InvariantCulture);
        return dbContext
            .Set<Tenant>()
            .AsNoTracking()
            .Where(ti => ti.Identifier == tenantIdentifier)
            .Select(ti => new TenantInfo
            {
                Id = ti.Id.ToString(),
                Name = ti.Name,
                Identifier = (string)ti.Identifier,
            })
            .SingleOrDefaultAsync();
    }

    public Task<bool> TryAddAsync(TenantInfo tenantInfo) => throw new InvalidOperationException("Not implemented");

    public Task<bool> TryRemoveAsync(string identifier) => throw new InvalidOperationException("Not implemented");

    public Task<bool> TryUpdateAsync(TenantInfo tenantInfo) => throw new InvalidOperationException("Not implemented");
}
