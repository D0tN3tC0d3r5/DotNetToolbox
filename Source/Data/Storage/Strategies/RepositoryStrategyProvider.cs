namespace DotNetToolbox.Data.Strategies;

public class RepositoryStrategyProvider
    : IRepositoryStrategyContainer
    , IRepositoryStrategyProvider {

    public Dictionary<Type, RepositoryStrategyEntry> Entries { get; } = [];

    public void TryAdd<TStrategy>(Func<string, TStrategy>? create = null)
        where TStrategy : class, IRepositoryStrategy, new() {
        var strategyType = typeof(TStrategy);
        var strategyInterface = strategyType.GetInterface(typeof(IValueObjectRepositoryStrategy<>).Name)!;
        var itemType = strategyInterface.GetGenericArguments().First();
        if (Entries.TryGetValue(itemType, out var entry)) {
            if (entry.StrategyType == strategyType)
                return;
            throw new InvalidOperationException($"A different repository strategy for '{itemType.Name}' is already registered.");
        }
        create ??= name => InstanceFactory.Create<TStrategy>(name);
        Entries[itemType] = new(strategyType, create);
    }

    public IRepositoryStrategy GetStrategy<TItem>(string name)
        => CreateStrategyFor<TItem>(name) as IRepositoryStrategy
        ?? throw new InvalidOperationException($"There is no strategy registered for '{typeof(TItem)}'.");

    private object? CreateStrategyFor<TItem>(string name) {
        var entry = Entries.GetValueOrDefault(typeof(TItem));
        return entry?.Create(name);
    }
}
