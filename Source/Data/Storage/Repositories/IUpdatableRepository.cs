namespace DotNetToolbox.Data.Repositories;

public interface IUpdatableRepository;

public interface IUpdatableRepository<TItem>
    : IUpdatableRepository {
    #region Blocking

    Result<TItem> Create(Action<TItem> setItem, IContext? validationContext = null);

    Result Seed(IEnumerable<TItem> seed, bool preserveContent = false, IContext? validationContext = null);

    Result Add(TItem newItem, IContext? validationContext = null);
    Result AddMany(IEnumerable<TItem> newItems, IContext? validationContext = null);

    Result Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IContext? validationContext = null);
    Result UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IContext? validationContext = null);

    Result AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IContext? validationContext = null);
    Result AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items, IContext? validationContext = null);

    Result Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IContext? validationContext = null);
    Result PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IContext? validationContext = null);

    Result Remove(Expression<Func<TItem, bool>> predicate);
    Result RemoveMany(Expression<Func<TItem, bool>> predicate);

    Result Clear();

    #endregion

    #region Async

    Task<Result> SeedAsync(IEnumerable<TItem> seed, bool preserveContent = false, IContext? validationContext = null, CancellationToken ct = default);

    Task<Result<TItem>> CreateAsync(Func<TItem, CancellationToken, Task> setItem, IContext? validationContext = null, CancellationToken ct = default);
    Task<Result> AddAsync(TItem newItem, IContext? validationContext = null, CancellationToken ct = default);
    Task<Result> AddManyAsync(IEnumerable<TItem> newItems, IContext? validationContext = null, CancellationToken ct = default);

    Task<Result> UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IContext? validationContext = null, CancellationToken ct = default);

    Task<Result> AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IContext? validationContext = null, CancellationToken ct = default);
    Task<Result> AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IContext? validationContext = null, CancellationToken ct = default);

    Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IContext? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IContext? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IContext? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IContext? validationContext = null, CancellationToken ct = default);

    Task<Result> RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<Result> RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<Result> ClearAsync(CancellationToken ct = default);

    #endregion
}

public interface IUpdatableRepository<TItem, in TKey>
    : IUpdatableRepository<TItem>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    #region Blocking

    Result Update(TItem updatedItem, IContext? validationContext = null);
    Result UpdateMany(IEnumerable<TItem> updatedItems, IContext? validationContext = null);

    Result AddOrUpdate(TItem updatedItem, IContext? validationContext = null);
    Result AddOrUpdateMany(IEnumerable<TItem> updatedItems, IContext? validationContext = null);

    Result Patch(TKey key, Action<TItem> setItem, IContext? validationContext = null);
    Result PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem, IContext? validationContext = null);

    Result Remove(TKey key);
    Result RemoveMany(IEnumerable<TKey> keys);

    #endregion

    #region Async

    Task<Result> UpdateAsync(TItem updatedItem, IContext? validationContext = null, CancellationToken ct = default);
    Task<Result> UpdateManyAsync(IEnumerable<TItem> updatedItems, IContext? validationContext = null, CancellationToken ct = default);

    Task<Result> AddOrUpdateAsync(TItem updatedItem, IContext? validationContext = null, CancellationToken ct = default);
    Task<Result> AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, IContext? validationContext = null, CancellationToken ct = default);

    Task<Result> PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, IContext? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, IContext? validationContext = null, CancellationToken ct = default);
    Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, IContext? validationContext = null, CancellationToken ct = default);

    Task<Result> RemoveAsync(TKey key, CancellationToken ct = default);
    Task<Result> RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default);

    #endregion
}
