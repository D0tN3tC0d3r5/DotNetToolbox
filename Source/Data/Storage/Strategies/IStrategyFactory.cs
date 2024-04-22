namespace DotNetToolbox.Data.Strategies;
public interface IStrategyFactory {
    void RegisterStrategy<TStrategy, TItem>()
        where TStrategy : class, IRepositoryStrategy<TItem>;
    void RegisterAsyncStrategy<TStrategy, TItem>()
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>;
    IRepositoryStrategy<TItem>? GetStrategy<TItem>(IEnumerable<TItem> data);
    TStrategy? GetStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IRepositoryStrategy<TItem>;
    TStrategy GetRequiredStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IRepositoryStrategy<TItem>;
    IAsyncRepositoryStrategy<TItem>? GetAsyncStrategy<TItem>(IEnumerable<TItem> data);
    TStrategy? GetAsyncStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>;
    TStrategy GetRequiredAsyncStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>;
}
