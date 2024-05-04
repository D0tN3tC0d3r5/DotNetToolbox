namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategy
    : IQueryableRepository
    , IUpdatableRepository {
    void SetRepository(IRepository repository);
}

public interface IRepositoryStrategy<TItem>
    : IRepositoryStrategy
    , IPagedQueryableRepository<TItem>
    , IChunkedQueryableRepository<TItem>
    , IUpdatableRepository<TItem> {
}
