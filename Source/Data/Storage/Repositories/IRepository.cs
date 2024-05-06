namespace DotNetToolbox.Data.Repositories;

public interface IRepository
    : IQueryableRepository
    , IReadOnlyRepository
    , IUpdatableRepository {
}

public interface IRepository<TItem>
    : IRepository
    , IQueryableRepository<TItem>
    , IReadOnlyRepository<TItem>
    , IUpdatableRepository<TItem>;

public interface IRepository<TItem, TKey>
    : IRepository<TItem>
    , IReadOnlyRepository<TItem, TKey>
    , IUpdatableRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    void SetKeyHandler(IKeyHandler<TKey> keyHandler);
}
