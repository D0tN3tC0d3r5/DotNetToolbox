namespace DotNetToolbox.Data.Strategies;

public class RepositoryStrategyProvider
    : IRepositoryStrategyContainer
    , IRepositoryStrategyProvider {

    public Dictionary<Type, RepositoryStrategyEntry> Entries { get; } = [];

    public void TryAdd<TStrategy>(Func<TStrategy>? create = null)
        where TStrategy : class, IRepositoryStrategy, new() {
        var strategyType = typeof(TStrategy);
        var strategyInterface = strategyType.GetInterface(typeof(IValueObjectRepositoryStrategy<>).Name)!;
        var itemType = strategyInterface.GetGenericArguments().First();
        if (Entries.TryGetValue(itemType, out var entry)) {
            if (entry.StrategyType == strategyType) return;
            throw new InvalidOperationException($"A different repository strategy for '{itemType.Name}' is already registered.");
        }
        create ??= Activator.CreateInstance<TStrategy>;
        Entries[itemType] = new(strategyType, create);
    }

    public IRepositoryStrategy GetStrategy<TItem>()
        => CreateStrategyFor<TItem>() as IRepositoryStrategy
        ?? throw new InvalidOperationException($"There is no strategy registered for '{typeof(TItem)}'.");

    private object? CreateStrategyFor<TItem>() {
        var entry = Entries.GetValueOrDefault(typeof(TItem));
        return entry?.Create();
    }
}
