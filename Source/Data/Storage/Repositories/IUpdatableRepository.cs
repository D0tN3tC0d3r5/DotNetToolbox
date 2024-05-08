namespace DotNetToolbox.Data.Repositories;

public interface IUpdatableRepository;

public interface IUpdatableRepository<TItem>
    : IUpdatableRepository {

    #region Blocking

    TItem Create(Action<TItem> setItem);

    void Seed(IEnumerable<TItem> seed);

    void Add(TItem newItem);
    void AddMany(IEnumerable<TItem> newItems);

    void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem);
    void UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems);

    void AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem);
    void AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items);

    void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem);
    void PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem);

    void Remove(Expression<Func<TItem, bool>> predicate);
    void RemoveMany(Expression<Func<TItem, bool>> predicate);

    void Clear();

    #endregion

    #region Async

    Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default);

    Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default);
    Task AddAsync(TItem newItem, CancellationToken ct = default);
    Task AddManyAsync(IEnumerable<TItem> newItems, CancellationToken ct = default);

    Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default);

    Task AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default);
    Task AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, CancellationToken ct = default);

    Task PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, CancellationToken ct = default);
    Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default);
    Task PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, CancellationToken ct = default);
    Task PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default);

    Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task ClearAsync(CancellationToken ct = default);

    #endregion
}

public interface IUpdatableRepository<TItem, in TKey>
    : IUpdatableRepository<TItem>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    #region Blocking

    void Update(TItem updatedItem);
    void UpdateMany(IEnumerable<TItem> updatedItems);

    void AddOrUpdate(TItem updatedItem);
    void AddOrUpdateMany(IEnumerable<TItem> updatedItems);

    void Patch(TKey key, Action<TItem> setItem);
    void PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem);

    void Remove(TKey key);
    void RemoveMany(IEnumerable<TKey> keys);

    #endregion

    #region Async

    Task UpdateAsync(TItem updatedItem, CancellationToken ct = default);
    Task UpdateManyAsync(IEnumerable<TItem> updatedItems, CancellationToken ct = default);

    Task AddOrUpdateAsync(TItem updatedItem, CancellationToken ct = default);
    Task AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, CancellationToken ct = default);

    Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default);
    Task PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, CancellationToken ct = default);
    Task PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default);

    Task RemoveAsync(TKey key, CancellationToken ct = default);
    Task RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default);

    #endregion
}
