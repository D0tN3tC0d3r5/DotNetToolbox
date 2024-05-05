namespace DotNetToolbox.Data.Repositories;

public interface IRepository<TItem>
    : IRepositoryBase<TItem>
    , IQueryableRepository<TItem>
    , IUpdatableRepository<TItem> {
}

public interface IRepository<TItem, TKey>
    : IRepository<TItem>
    , IQueryableRepository<TItem, TKey>
    , IUpdatableRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    void SetKeyHandler(IKeyHandler<TKey> keyHandler);
}
