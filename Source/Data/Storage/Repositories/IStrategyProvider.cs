namespace DotNetToolbox.Data.Repositories;

public interface IStrategyProvider {
    void RegisterStrategy<TModel>(IRepositoryStrategy strategy);
    IRepositoryStrategy<TModel>? GetStrategy<TModel>();
}
