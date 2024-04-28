namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategyProvider {
    IRepositoryStrategy<TItem> GetStrategyFor<TItem>();
}
