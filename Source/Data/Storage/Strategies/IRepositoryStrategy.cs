namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategy
    : IQueryableRepository
    , IUpdatableRepository {
    void SetRepository(IRepositoryBase repository);
}

public interface IRepositoryStrategy<TItem>
    : IRepositoryStrategy
    , IPagedRepository<TItem>
    , IChunkedRepository<TItem>
    , IUpdatableRepository<TItem>;

public interface IRepositoryStrategy<TItem, TKey>
    : IRepositoryStrategy<TItem>
    , IQueryableRepository<TItem, TKey>
    , IUpdatableRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    void SetKeyHandler(IKeyHandler<TKey> keyHandler);
};
