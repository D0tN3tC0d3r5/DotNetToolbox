namespace DotNetToolbox.Data.Repositories;

public interface IStrategyFactory {
    void RegisterStrategy<TItem, TStrategy>()
        where TStrategy : class, IRepositoryStrategy<TItem>
        where TItem : class;
    RepositoryStrategy<TItem>? GetRepositoryStrategy<TItem>(IEnumerable<TItem> data)
        where TItem : class;
    AsyncRepositoryStrategy<TItem>? GetAsyncRepositoryStrategy<TItem>(IEnumerable<TItem> data)
        where TItem : class;
}
