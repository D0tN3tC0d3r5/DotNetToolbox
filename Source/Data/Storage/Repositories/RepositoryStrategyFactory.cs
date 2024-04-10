namespace DotNetToolbox.Data.Repositories;

public class RepositoryStrategyFactory
    : IRepositoryStrategyFactory {
    public IRepositoryStrategy Create<TStrategy>()
        where TStrategy : class, IRepositoryStrategy
        => InstanceFactory.Create<TStrategy>();
}
