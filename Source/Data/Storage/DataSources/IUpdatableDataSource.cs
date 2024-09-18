namespace DotNetToolbox.Data.DataSources;

public interface IUpdatableDataSource;

public interface IUpdatableDataSource<TItem>
    : IUpdatableDataSource {
    #region Blocking

    Result<TItem> Create(Action<TItem>? setItem = null, IMap? validationContext = null);

    Result Seed(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null);

    Result Add(TItem newItem, IMap? validationContext = null);
    Result AddMany(IEnumerable<TItem> newItems, IMap? validationContext = null);

    Result Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null);
    Result UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null);

    Result AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null);
    Result AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items, IMap? validationContext = null);

    Result Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null);
    Result PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null);

    Result Remove(Expression<Func<TItem, bool>> predicate);
    Result RemoveMany(Expression<Func<TItem, bool>> predicate);

    Result Clear();

    #endregion

    #region Async

    Task<Result> SeedAsync(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result<TItem>> CreateAsync(Func<TItem, CancellationToken, Task> setItem, IMap validationContext, CancellationToken ct = default);
    Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default);
    Task<Result> AddAsync(TItem newItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> AddManyAsync(IEnumerable<TItem> newItems, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result> UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result> AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result> RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<Result> RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<Result> ClearAsync(CancellationToken ct = default);

    #endregion
}

public interface IUpdatableDataSource<TItem, in TKey>
    : IUpdatableDataSource<TItem>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    #region Blocking

    Result Update(TItem updatedItem, IMap? validationContext = null);
    Result UpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null);

    Result AddOrUpdate(TItem updatedItem, IMap? validationContext = null);
    Result AddOrUpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null);

    Result Patch(TKey key, Action<TItem> setItem, IMap? validationContext = null);
    Result PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null);

    Result Remove(TKey key);
    Result RemoveMany(IEnumerable<TKey> keys);

    #endregion

    #region Async

    Task<Result> UpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> UpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result> AddOrUpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result> PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result> RemoveAsync(TKey key, CancellationToken ct = default);
    Task<Result> RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default);

    #endregion
}