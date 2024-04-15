namespace DotNetToolbox.Data.Repositories;

public interface IStrategyProvider {
    void RegisterStrategy<TItem>(IQueryStrategy<TItem> strategy)
        where TItem : class;
    IQueryStrategy<TItem>? GetStrategy<TItem>()
        where TItem : class;
    IQueryStrategy? GetStrategy(Type modelType);
}
