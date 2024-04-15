namespace DotNetToolbox.Data.Repositories;

public class StrategyProvider
    : IStrategyProvider {
    private readonly Dictionary<Type, IQueryStrategy> _strategies = [];

    public void RegisterStrategy<TItem>(IQueryStrategy<TItem> strategy)
        where TItem : class
        => _strategies[typeof(TItem)] = strategy;

    public IQueryStrategy<TItem>? GetStrategy<TItem>()
        where TItem : class
        => (IQueryStrategy<TItem>?)GetStrategy(typeof(TItem));

    public IQueryStrategy? GetStrategy(Type modelType)
        => _strategies.GetValueOrDefault(modelType);
}
