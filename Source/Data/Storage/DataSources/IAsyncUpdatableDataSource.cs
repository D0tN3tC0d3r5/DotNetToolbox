namespace DotNetToolbox.Data.DataSources;

public interface IAsyncUpdatableDataSource;

public interface IAsyncUpdatableDataSource<TItem, in TKey>
    : IAsyncUpdatableDataSource
    where TItem : IEntity<TKey>
    where TKey : notnull {
    Task<Result> SeedAsync(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result<TItem>> CreateAsync(Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);
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
    Task<Result> UpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> UpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result> AddOrUpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result> PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);

    Task<Result> RemoveAsync(TKey key, CancellationToken ct = default);
    Task<Result> RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default);
}
