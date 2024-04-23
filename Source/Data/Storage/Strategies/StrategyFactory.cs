namespace DotNetToolbox.Data.Strategies;
public class StrategyFactory
    : IStrategyFactory {
    private readonly Dictionary<Type, Type> _strategies = [];
    public void RegisterStrategy<TItem, TStrategy>()
        where TStrategy : class, IRepositoryStrategy<TItem>
        => _strategies[typeof(TItem)] = typeof(TStrategy);
    public void RegisterAsyncStrategy<TItem, TStrategy>()
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>
        => _strategies[typeof(TItem)] = typeof(TStrategy);

    public IRepositoryStrategy<TItem>? GetStrategy<TItem>()
        => CreateStrategy<TItem>() as IRepositoryStrategy<TItem>;
    public TStrategy? GetStrategy<TItem, TStrategy>()
        where TStrategy : class, IRepositoryStrategy<TItem>
        => GetStrategy<TItem>() as TStrategy;
    public TStrategy GetRequiredStrategy<TItem, TStrategy>()
        where TStrategy : class, IRepositoryStrategy<TItem>
        => GetStrategy<TItem, TStrategy>()
        ?? throw new InvalidOperationException($"There is no '{typeof(TStrategy).Name}' registered for '{typeof(TItem)}'.");

    public IAsyncRepositoryStrategy<TItem>? GetAsyncStrategy<TItem>()
        => CreateStrategy<TItem>() as IAsyncRepositoryStrategy<TItem>;
    public TStrategy? GetAsyncStrategy<TItem, TStrategy>()
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>
        => GetAsyncStrategy<TItem>() as TStrategy;
    public TStrategy GetRequiredAsyncStrategy<TItem, TStrategy>()
        where TStrategy : class, IAsyncRepositoryStrategy<TItem>
        => GetAsyncStrategy<TItem, TStrategy>()
        ?? throw new InvalidOperationException($"There is no '{typeof(TStrategy).Name}' registered for '{typeof(TItem)}'.");

    public object? CreateStrategy<TItem>(){
        var strategyType = _strategies.GetValueOrDefault(typeof(TItem));
        return strategyType is null
            ? null
            : Activator.CreateInstance(strategyType);
    }
}
