namespace DotNetToolbox.Data.Repositories;

public interface IStrategyProvider {
    void RegisterStrategy<TModel>(IQueryableStrategy strategy);
    IQueryableStrategy? GetStrategy(Type modelType);
    IQueryableStrategy<TModel>? GetStrategy<TModel>();
}
