namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategy
    : IReadOnlyRepository
    , IUpdatableRepository
    , IAsyncDisposable;

public interface IRepositoryStrategy<TItem>
    : IRepositoryStrategy
    , IReadOnlyRepository<TItem>
    , IUpdatableRepository<TItem>;

public interface IRepositoryStrategy<TItem, TKey>
    : IRepositoryStrategy<TItem>
    , IReadOnlyRepository<TItem, TKey>
    , IUpdatableRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull;
