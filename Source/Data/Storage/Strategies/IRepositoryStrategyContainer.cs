namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategyContainer {
    void TryAdd<TStrategy, TImplementation>()
        where TStrategy : class, IRepositoryStrategy
        where TImplementation : class, TStrategy, new();
    public void TryAdd<TStrategy>(Func<TStrategy>? create = null)
        where TStrategy : class, IRepositoryStrategy, new();
}
