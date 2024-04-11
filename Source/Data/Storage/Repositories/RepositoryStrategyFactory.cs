namespace DotNetToolbox.Data.Repositories;

public class RepositoryStrategyFactory
    : IRepositoryStrategyFactory {
    public TStrategy Create<TItem, TStrategy>(IItemSet<TItem, TStrategy> repository)
        where TStrategy : class, IQueryStrategy<TStrategy>
        => InstanceFactory.Create<TStrategy>(repository);
}
