namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategy
    : IReadOnlyRepository
    , IUpdatableRepository {
    void SetRepository(IQueryableRepository repository);
}

public interface IRepositoryStrategy<TItem>
    : IRepositoryStrategy
    , IReadOnlyRepository<TItem>
    , IUpdatableRepository<TItem>;

public interface IRepositoryStrategy<TItem, TKey>
    : IRepositoryStrategy<TItem>
    , IReadOnlyRepository<TItem, TKey>
    , IUpdatableRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    void SetKeyHandler(IKeyHandler<TKey> keyHandler);
};
