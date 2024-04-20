namespace DotNetToolbox.Data.Strategies;
public class StrategyFactory
    : IStrategyFactory {
    private readonly Dictionary<Type, Type> _strategies = [];
    public void RegisterStrategy<TStrategy, TItem>()
        where TStrategy : class, IRepositoryStrategy<TItem>
        => _strategies[typeof(TItem)] = typeof(TStrategy);
    public void RegisterAsyncStrategy<TStrategy, TItem>()
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>
        => _strategies[typeof(TItem)] = typeof(TStrategy);

    public IRepositoryStrategy<TItem>? GetStrategy<TItem>(IEnumerable<TItem> data)
        => CreateStrategy(data) as IRepositoryStrategy<TItem>;
    public TStrategy? GetStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IRepositoryStrategy<TItem>
        => GetStrategy(data) as TStrategy;
    public TStrategy GetRequiredStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IRepositoryStrategy<TItem>
        => GetStrategy<TStrategy, TItem>(data)
        ?? throw new InvalidOperationException($"There is no '{typeof(TStrategy).Name}' registered for '{typeof(TItem)}'.");

    public IAsyncRepositoryStrategy<TItem>? GetAsyncStrategy<TItem>(IEnumerable<TItem> data)
        => CreateStrategy(data) as IAsyncRepositoryStrategy<TItem>;
    public TStrategy? GetAsyncStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>
        => GetAsyncStrategy(data) as TStrategy;
    public TStrategy GetRequiredAsyncStrategy<TStrategy, TItem>(IEnumerable<TItem> data)
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>
        => GetAsyncStrategy<TStrategy, TItem>(data)
        ?? throw new InvalidOperationException($"There is no '{typeof(TStrategy).Name}' registered for '{typeof(TItem)}'.");

    public object? CreateStrategy<TItem>(IEnumerable<TItem> data) {
        var strategyType = _strategies.GetValueOrDefault(typeof(TItem));
        return strategyType is null
            ? null
            : Activator.CreateInstance(strategyType, data);
    }
}
