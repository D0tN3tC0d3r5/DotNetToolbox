namespace DotNetToolbox.Data.Repositories;

public interface IRepositoryStrategyFactory {
    IRepositoryStrategy Create<TStrategy>()
        where TStrategy : class, IRepositoryStrategy;
}
