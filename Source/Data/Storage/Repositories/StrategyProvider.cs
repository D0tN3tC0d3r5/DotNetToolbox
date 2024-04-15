namespace DotNetToolbox.Data.Repositories;

public class StrategyProvider
    : IStrategyProvider {
    private readonly Dictionary<Type, IQueryableStrategy> _strategies = [];

    public void RegisterStrategy<TItem>(IQueryableStrategy strategy)
        where TItem : class, new()
        => _strategies[typeof(TItem)] = strategy;

    public IQueryableStrategy? GetStrategy<TItem>()
        where TItem : class, new()
        => GetStrategy(typeof(TItem));

    public IQueryableStrategy? GetStrategy(Type modelType)
        => _strategies.GetValueOrDefault(modelType);
}
