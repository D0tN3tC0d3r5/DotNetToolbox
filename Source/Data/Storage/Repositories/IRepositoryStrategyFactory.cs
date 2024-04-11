namespace DotNetToolbox.Data.Repositories;

public interface IRepositoryStrategyFactory {
    TStrategy Create<TEntity, TStrategy>(IItemSet<TEntity, TStrategy> repository)
        where TStrategy : class, IQueryStrategy<TStrategy>;
}
