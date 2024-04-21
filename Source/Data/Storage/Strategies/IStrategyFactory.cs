namespace DotNetToolbox.Data.Strategies;
public interface IStrategyFactory {
    void RegisterStrategy<TStrategy, TItem>()
        where TStrategy : class, IRepositoryStrategy<TItem>
        where TItem : class;
    void RegisterAsyncStrategy<TStrategy, TItem>()
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>
        where TItem : class;
    IRepositoryStrategy<TItem>? GetStrategy<TItem>(IEnumerable<TItem> data)
        where TItem : class;
    TStrategy? GetStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IRepositoryStrategy<TItem>
        where TItem : class;
    TStrategy GetRequiredStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IRepositoryStrategy<TItem>
        where TItem : class;
    IAsyncRepositoryStrategy<TItem>? GetAsyncStrategy<TItem>(IEnumerable<TItem> data)
        where TItem : class;
    TStrategy? GetAsyncStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>
        where TItem : class;
    TStrategy GetRequiredAsyncStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>
        where TItem : class;
}
