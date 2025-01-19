using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RioScaffolding.OpenSpacePlanner.Common.Persistence;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Exceptions;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.StartupExtensions;

public static class MultiTenancy
{
    private const string TenantToken = "__tenant__";

    public static void AddMultiTenancy<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext =>
        services
            .AddMultiTenant<TenantInfo>()
            .WithRouteStrategy(TenantToken)
            .WithStore<EntityFrameworkTenantStore<TDbContext>>(ServiceLifetime.Scoped);

    public static void UseMultiTenancy(this IApplicationBuilder app) => app.UseMiddleware<MultiTenantMiddleware>();

    public class MultiTenantMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.RequestServices.GetRequiredService<IMultiTenantContextAccessor>();
            var mtcSetter = context.RequestServices.GetRequiredService<IMultiTenantContextSetter>();

            var resolver = context.RequestServices.GetRequiredService<ITenantResolver>();

            var multiTenantContext = await resolver.ResolveAsync(context);
            var tenantInfo = multiTenantContext.TenantInfo;

            if (tenantInfo?.Identifier is null && context.Request.RouteValues[TenantToken] is not null)
            {
                throw new TenantNotFoundException();
            }

            mtcSetter.MultiTenantContext = multiTenantContext;
            context.Items[typeof(IMultiTenantContext)] = multiTenantContext;

            var tenantDatabaseContext =
                context.RequestServices.GetRequiredService<DbContext>() as TenantApplicationDbContextBase;
            if (tenantDatabaseContext is not null)
            {
                // make sure to update the current database context if we created one to check the tenant store
                tenantDatabaseContext.TenantInfo = tenantInfo;
            }

            await next(context);
        }
    }
}
