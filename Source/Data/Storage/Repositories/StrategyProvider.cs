namespace DotNetToolbox.Data.Repositories;

public class StrategyProvider
    : IStrategyProvider {
    private readonly Dictionary<Type, IQueryableStrategy> _strategies = [];

    public void RegisterStrategy<TModel>(IQueryableStrategy strategy)
        => _strategies[typeof(TModel)] = strategy;

    public IQueryableStrategy<TModel>? GetStrategy<TModel>()
        => (IQueryableStrategy<TModel>?)GetStrategy(typeof(TModel));

    public IQueryableStrategy? GetStrategy(Type modelType)
        => _strategies.GetValueOrDefault(modelType);
}
