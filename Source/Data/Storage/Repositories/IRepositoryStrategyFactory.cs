namespace DotNetToolbox.Data.Repositories;

public interface IRepositoryStrategyFactory {
    IRepositoryStrategy Create<TStrategy, TEntity>(IEnumerable<TEntity> source)
        where TStrategy : class, IRepositoryStrategy;
}
