namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategyProvider {
    TStrategy? GetStrategy<TStrategy>()
        where TStrategy : class, IRepositoryStrategy;
    IRepositoryStrategy<TItem>? GetStrategyFor<TItem>();

    TStrategy GetRequiredStrategy<TStrategy>()
        where TStrategy : class, IRepositoryStrategy;
    IRepositoryStrategy<TItem> GetRequiredStrategyFor<TItem>();
    IUnitOfWorkRepositoryStrategy<TItem> GetRequiredUnitOfWorkStrategyFor<TItem>();
    IAsyncRepositoryStrategy<TItem> GetRequiredAsyncStrategyFor<TItem>();
    IAsyncUnitOfWorkRepositoryStrategy<TItem> GetRequiredAsyncUnitOfWorkStrategyFor<TItem>();
}
