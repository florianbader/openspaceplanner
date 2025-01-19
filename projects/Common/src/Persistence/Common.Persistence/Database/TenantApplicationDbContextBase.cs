using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;

public abstract class TenantApplicationDbContextBase : ApplicationDbContextBase, IMultiTenantDbContext
{
    /// <inheritdoc />
    public ITenantInfo? TenantInfo { get; set; }

    /// <inheritdoc />
    public TenantMismatchMode TenantMismatchMode { get; set; } = TenantMismatchMode.Throw;

    /// <inheritdoc />
    public TenantNotSetMode TenantNotSetMode { get; set; } = TenantNotSetMode.Overwrite;

    protected TenantApplicationDbContextBase(
        IMultiTenantContextAccessor multiTenantContextAccessor,
        DbContextOptions options
    )
        : base(options) => TenantInfo = multiTenantContextAccessor.MultiTenantContext.TenantInfo;

    protected TenantApplicationDbContextBase(TenantInfo? tenantInfo, DbContextOptions options)
        : base(options) => TenantInfo = tenantInfo;

    protected TenantApplicationDbContextBase(DbContextOptions options)
        : base(options) { }

    /// <inheritdoc />
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.EnforceMultiTenant();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <inheritdoc />
    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        this.EnforceMultiTenant();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
