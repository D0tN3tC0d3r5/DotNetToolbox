namespace DotNetToolbox.Data.Repositories;

public class StrategyFactory
    : IStrategyFactory {
    private readonly Dictionary<Type, Type> _strategies = [];

    public void RegisterStrategy<TItem, TStrategy>()
        where TStrategy : class, IRepositoryStrategy<TItem>
        where TItem : class
        => _strategies[typeof(TItem)] = typeof(TStrategy);

    public RepositoryStrategy<TItem>? GetRepositoryStrategy<TItem>(IQueryable<TItem> data)
        where TItem : class
        => CreateStrategy(data) as RepositoryStrategy<TItem>;

    public AsyncRepositoryStrategy<TItem>? GetAsyncRepositoryStrategy<TItem>(IQueryable<TItem> data)
        where TItem : class
        => CreateStrategy(data) as AsyncRepositoryStrategy<TItem>;

    public object? CreateStrategy<TItem>(IQueryable<TItem> data)
        where TItem : class {
        var strategyType = _strategies.GetValueOrDefault(typeof(TItem));
        if (strategyType is null) return null;
        return Activator.CreateInstance(strategyType, data);
    }
}
