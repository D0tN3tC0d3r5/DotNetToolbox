namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategy
    : IQueryableRepository
    , IUpdatableRepository;

public interface IRepositoryStrategy<TItem>
    : IRepositoryStrategy
    , IRepository<TItem>;
