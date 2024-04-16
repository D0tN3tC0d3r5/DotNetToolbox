namespace DotNetToolbox.Data.Repositories;

public class StrategyProvider
    : IStrategyProvider {
    private readonly Dictionary<Type, IQueryStrategy> _strategies = [];

    public void RegisterStrategy<TItem>(IQueryStrategy strategy)
        where TItem : class
        => _strategies[typeof(TItem)] = IsNotNull(strategy);

    public IQueryStrategy? GetStrategy<TItem>()
        where TItem : class
        => GetStrategy(typeof(TItem));

    public IQueryStrategy? GetStrategy(Type modelType)
        => _strategies.GetValueOrDefault(modelType);
}
