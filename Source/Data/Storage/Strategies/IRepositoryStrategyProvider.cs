namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategyProvider {
    IRepositoryStrategy GetStrategy<TItem>();
}
