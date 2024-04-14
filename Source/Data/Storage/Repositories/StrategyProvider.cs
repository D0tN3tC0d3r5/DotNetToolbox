namespace DotNetToolbox.Data.Repositories;

public class StrategyProvider
    : IStrategyProvider {
    private readonly Dictionary<Type, IRepositoryStrategy> _strategies = [];

    public void RegisterStrategy<TModel>(IRepositoryStrategy strategy)
        => _strategies[typeof(TModel)] = strategy;

    public IRepositoryStrategy<TModel>? GetStrategy<TModel>()
        => _strategies.TryGetValue(typeof(TModel), out var strategy)
               ? (IRepositoryStrategy<TModel>)strategy
               : null;
}
