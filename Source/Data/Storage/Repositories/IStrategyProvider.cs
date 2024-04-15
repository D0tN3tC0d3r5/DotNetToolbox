namespace DotNetToolbox.Data.Repositories;

public interface IStrategyProvider {
    void RegisterStrategy<TItem>(IQueryableStrategy strategy)
        where TItem : class, new();
    IQueryableStrategy? GetStrategy<TItem>()
        where TItem : class, new();
    IQueryableStrategy? GetStrategy(Type modelType);
}
