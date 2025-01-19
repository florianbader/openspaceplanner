using Microsoft.EntityFrameworkCore;
using RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Exceptions;

namespace RioScaffolding.OpenSpacePlanner.Common.WebApi.Application.Extensions;

public static class ApplicationDbContextExtensions
{
    public static async Task<TEntity> FindOrThrowAsync<TEntity>(
        this DbSet<TEntity> dbSet,
        object keyValue,
        CancellationToken cancellationToken
    )
        where TEntity : class =>
        await dbSet.FindAsync([keyValue], cancellationToken) ?? throw new EntityNotFoundException();
}
