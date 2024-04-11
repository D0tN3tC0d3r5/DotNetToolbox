namespace DotNetToolbox.Data.Repositories;

public class RepositoryStrategyFactory
    : IRepositoryStrategyFactory {
    public IRepositoryStrategy Create<TStrategy, TEntity>(IEnumerable<TEntity>? source = null)
        where TStrategy : class, IRepositoryStrategy
        => InstanceFactory.Create<TStrategy>(source);
}
