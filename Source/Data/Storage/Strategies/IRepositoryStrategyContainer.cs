namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategyContainer {
    public void TryAdd<TStrategy>(Func<TStrategy>? create = null)
        where TStrategy : class, IRepositoryStrategy, new();
}
