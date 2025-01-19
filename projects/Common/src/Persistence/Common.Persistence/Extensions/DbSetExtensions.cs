using System.Linq.Expressions;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Extensions;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.Models;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Extensions;

public static class DbSetExtensions
{
    public static IQueryable<TEntity> AsQuery<TEntity>(this DbSet<TEntity> dbSet, string tag)
        where TEntity : class => dbSet.AsNoTracking().TagWithCallSite(tag);

    public static IQueryable<TEntity> TagWithCallSite<TEntity>(this DbSet<TEntity> dbSet, string tag)
        where TEntity : class => dbSet.TagWith(tag).TagWithCallSite();

    public static IQueryable<TEntity> TagWithCallSite<TEntity>(this IQueryable<TEntity> dbSet, string tag)
        where TEntity : class => dbSet.TagWith(tag).TagWithCallSite();

    public static async Task<ItemsResult<TResult>> GetItemsResultAsync<TEntity, TResult>(
        this IQueryable<TEntity> dbSet,
        GridifyQuery? query,
        Expression<Func<TEntity, TResult>> projectExpression,
        CancellationToken cancellationToken
    )
    {
        if (query is null)
        {
            return new ItemsResult<TResult>(await dbSet.Select(projectExpression).ToArrayAsync(cancellationToken));
        }

        var project = projectExpression.Compile();
        var paging = await dbSet.GridifyAsync(query, cancellationToken);
        var totalPages = (int)Math.Ceiling((double)paging.Count / query.PageSize);

        return new ItemsResult<TResult>(
            paging.Data.Select(project),
            new PageResult(query.PageSize, paging.Count, totalPages, query.Page)
        );
    }

    public static Task<ItemsResult<TEntity>> GetItemsResultAsync<TEntity>(
        this IQueryable<TEntity> dbSet,
        GridifyQuery? query,
        CancellationToken cancellationToken
    ) => dbSet.GetItemsResultAsync(query, (entity) => entity, cancellationToken);
}
