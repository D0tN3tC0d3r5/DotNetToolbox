namespace DotNetToolbox.Data.Strategies;
public interface IStrategyFactory {
    void RegisterStrategy<TItem, TStrategy>()
        where TStrategy : class, IRepositoryStrategy<TItem>;
    void RegisterAsyncStrategy<TItem, TStrategy>()
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>;

    IRepositoryStrategy<TItem>? GetStrategy<TItem>();
    TStrategy? GetStrategy<TItem, TStrategy>()
        where TStrategy : class, IRepositoryStrategy<TItem>;
    TStrategy GetRequiredStrategy<TItem, TStrategy>()
        where TStrategy : class, IRepositoryStrategy<TItem>;

    IAsyncRepositoryStrategy<TItem>? GetAsyncStrategy<TItem>();
    TStrategy? GetAsyncStrategy<TItem, TStrategy>()
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>;
    TStrategy GetRequiredAsyncStrategy<TItem, TStrategy>()
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>;
}
