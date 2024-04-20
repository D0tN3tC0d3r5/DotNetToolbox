namespace DotNetToolbox.Data.Strategies;

public interface IUnitOfWorkRepositoryStrategy
    : IRepositoryStrategy
    , IUnitOfWorkRepository;

public interface IUnitOfWorkRepositoryStrategy<TItem>
    : IRepositoryStrategy<TItem>
    , IUnitOfWorkRepositoryStrategy;
