namespace DotNetToolbox.Data.Repositories;

public interface IStrategyProvider {
    void RegisterStrategy<TItem>(IQueryableStrategy strategy)
        where TItem : class;
    IQueryableStrategy? GetStrategy<TItem>()
        where TItem : class;
    IQueryableStrategy? GetStrategy(Type modelType);
}
