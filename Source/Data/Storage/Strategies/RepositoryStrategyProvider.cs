namespace DotNetToolbox.Data.Strategies;

public class RepositoryStrategyProvider
    : IRepositoryStrategyContainer, IRepositoryStrategyProvider {
    private record RepositoryStrategyEntry(Type StrategyType, Func<IRepositoryStrategy> Create);

    private readonly Dictionary<Type, RepositoryStrategyEntry> _entries = [];
    public void TryAdd<TStrategy, TImplementation>()
        where TStrategy : class, IRepositoryStrategy
        where TImplementation : class, TStrategy, new()
        => TryAdd(() => new TImplementation());

    public void TryAdd<TStrategy>(Func<TStrategy>? create = null)
        where TStrategy : class, IRepositoryStrategy, new() {
        var strategyType = typeof(TStrategy);
        var itemType = strategyType.GetGenericArguments().FirstOrDefault();
        while (itemType is null && strategyType.BaseType is not null) {
            strategyType = strategyType.BaseType;
            itemType = strategyType.GetGenericArguments().FirstOrDefault();
        }
        if (itemType is null) throw new InvalidOperationException($"'{strategyType}' does not have a generic type argument.");
        create ??= Activator.CreateInstance<TStrategy>;
        _entries[itemType] = new(typeof(TStrategy), create);
    }

    public object? CreateStrategyFor<TItem>() {
        var entry = _entries.GetValueOrDefault(typeof(TItem));
        return entry?.Create();
    }
    public object? CreateStrategy<TStrategy>() {
        var entry = _entries.Values.FirstOrDefault(st => typeof(TStrategy).IsAssignableTo(st.StrategyType));
        return entry?.Create();
    }
    public TStrategy? GetStrategy<TStrategy>()
        where TStrategy : class, IRepositoryStrategy {
        var result = CreateStrategy<TStrategy>();
        return result as TStrategy;
    }
    public TStrategy GetRequiredStrategy<TStrategy>()
        where TStrategy : class, IRepositoryStrategy
        => GetStrategy<TStrategy>()
        ?? throw new InvalidOperationException($"The '{typeof(TStrategy).Name}' is not registered.");

    public IRepositoryStrategy<TItem>? GetStrategyFor<TItem>()
        => CreateStrategyFor<TItem>() as IRepositoryStrategy<TItem>;
    public IRepositoryStrategy<TItem> GetRequiredStrategyFor<TItem>()
        => GetStrategyFor<TItem>()
        ?? throw new InvalidOperationException($"There is no strategy registered for '{typeof(TItem)}'.");
    public IUnitOfWorkRepositoryStrategy<TItem> GetRequiredUnitOfWorkStrategyFor<TItem>()
        => GetStrategyFor<TItem>() as IUnitOfWorkRepositoryStrategy<TItem>
        ?? throw new InvalidOperationException($"There is no unit of work strategy registered for '{typeof(TItem)}'.");
    public IAsyncRepositoryStrategy<TItem> GetRequiredAsyncStrategyFor<TItem>()
        => GetStrategyFor<TItem>() as IAsyncRepositoryStrategy<TItem>
        ?? throw new InvalidOperationException($"There is no async strategy registered for '{typeof(TItem)}'.");
    public IAsyncUnitOfWorkRepositoryStrategy<TItem> GetRequiredAsyncUnitOfWorkStrategyFor<TItem>()
        => GetStrategyFor<TItem>() as IAsyncUnitOfWorkRepositoryStrategy<TItem>
        ?? throw new InvalidOperationException($"There is no async unit of work strategy registered for '{typeof(TItem)}'.");
}
