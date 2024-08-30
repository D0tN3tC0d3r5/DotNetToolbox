namespace DotNetToolbox.Data.Repositories;

[SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "Implemented below")]
public interface IRepository
    : IQueryableRepository
    , IReadOnlyRepository
    , IUpdatableRepository
    , IAsyncDisposable;

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
    where TKey : notnull;
