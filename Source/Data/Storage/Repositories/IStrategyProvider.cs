namespace DotNetToolbox.Data.Repositories;

public interface IStrategyProvider {
    void RegisterStrategy<TItem>(IQueryStrategy strategy)
        where TItem : class;
    IQueryStrategy? GetStrategy<TItem>()
        where TItem : class;
    IQueryStrategy? GetStrategy(Type modelType);
}
