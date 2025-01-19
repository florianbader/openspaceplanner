using Gridify;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

public interface IGridifyConfiguration<TEntity>
    where TEntity : class
{
    void Configure(GridifyMapper<TEntity> mapper);
}
