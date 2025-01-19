using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;

namespace RioScaffolding.OpenSpacePlanner.Persistence;

/// <inheritdoc />
public class TenantApplicationDbContext : TenantApplicationDbContextBase
{
    /// <inheritdoc />
    public TenantApplicationDbContext(DbContextOptions options)
        : base(options) { }

    /// <inheritdoc />
    public TenantApplicationDbContext(IMultiTenantContextAccessor multiTenantContextAccessor, DbContextOptions options)
        : base(multiTenantContextAccessor, options) { }

    /// <inheritdoc />
    public TenantApplicationDbContext(TenantInfo? tenantInfo, DbContextOptions options)
        : base(tenantInfo, options) { }
}
