namespace DotNetToolbox.Data.DataSources;

public class AsyncDataSource<TStorage, TItem>
    : AsyncDataSource<TStorage, TItem, uint>
    where TStorage : class, IAsyncStorage<TItem, uint>, new()
    where TItem : IEntity<uint> {
    public AsyncDataSource(string id, IAsyncKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        : base(id, keyGenerator, seed) { }
    public AsyncDataSource(IAsyncKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) { }
    public AsyncDataSource(string id, IEnumerable<TItem>? seed = null)
        : this(id, InMemoryAsyncKeyGenerator<uint>.Instance, seed) { }
    public AsyncDataSource(IEnumerable<TItem> seed)
        : this(InMemoryAsyncKeyGenerator<uint>.Instance, seed) { }
    public AsyncDataSource()
        : this([]) { }

    public AsyncDataSource(TStorage storage)
        : base(storage) { }
}

public class AsyncDataSource<TStorage, TItem, TKey>
    : AsyncQueryableDataSource<TStorage, TItem, TKey>
    , IAsyncDataSource<TItem, TKey>
    where TStorage : class, IAsyncStorage<TItem, TKey>, new()
    where TItem : IEntity<TKey>
    where TKey : notnull {
    public AsyncDataSource(string id, IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        : base(id, keyGenerator, seed) {
    }
    public AsyncDataSource(IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) {
    }
    public AsyncDataSource(string id, IEnumerable<TItem>? seed = null)
        : this(id, InMemoryAsyncKeyGenerator<TKey>.Instance, seed) { }
    public AsyncDataSource(IEnumerable<TItem> seed)
        : this(InMemoryAsyncKeyGenerator<TKey>.Instance, seed) { }
    public AsyncDataSource()
        : this([]) { }

    public AsyncDataSource(TStorage storage)
        : base(storage) {
    }

    public ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => Storage.FindByKeyAsync(key, ct);

    public Task<Result> UpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.UpdateAsync(updatedItem, validationContext, ct);

    public Task<Result> UpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.UpdateManyAsync(updatedItems, validationContext, ct);

    public Task<Result> AddOrUpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.AddOrUpdateAsync(updatedItem, validationContext, ct);

    public Task<Result> AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.AddOrUpdateManyAsync(updatedItems, validationContext, ct);

    public Task<Result> PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.PatchAsync(key, setItem, validationContext, ct);

    public Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.PatchManyAsync(keys, setItem, validationContext, ct);
    public Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.PatchManyAsync(keys, setItem, validationContext, ct);

    public Task<Result> RemoveAsync(TKey key, CancellationToken ct = default)
        => Storage.RemoveAsync(key, ct);

    public Task<Result> RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default)
        => Storage.RemoveManyAsync(keys, ct);

    public virtual Task<Result> SeedAsync(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.SeedAsync(seed, preserveContent, validationContext, ct);

    public virtual Task<Result> LoadAsync(CancellationToken ct = default)
        => Storage.LoadAsync(ct);

    public virtual ValueTask<TItem[]> GetAllAsync(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default)
        => Storage.GetAllAsync(filterBy, orderBy, ct);

    public virtual ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default)
        => Storage.GetPageAsync(pageIndex, pageSize, filterBy, orderBy, ct);
    public virtual ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default)
        => Storage.GetChunkAsync(isChunkStart, blockSize, filterBy, orderBy, ct);

    public virtual ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Storage.FindAsync(predicate, ct);

    public virtual Task<Result<TItem>> CreateAsync(Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.CreateAsync(setItem, validationContext, ct);
    public virtual Task<Result> AddAsync(TItem newItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.AddAsync(newItem, validationContext, ct);
    public virtual Task<Result> AddManyAsync(IEnumerable<TItem> newItems, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.AddManyAsync(newItems, validationContext, ct);

    public virtual Task<Result> UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.UpdateAsync(predicate, updatedItem, validationContext, ct);

    public virtual Task<Result> AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.AddOrUpdateAsync(predicate, updatedItem, validationContext, ct);
    public virtual Task<Result> AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.AddOrUpdateManyAsync(predicate, updatedItems, validationContext, ct);

    public virtual Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.PatchAsync(predicate, setItem, validationContext, ct);
    public virtual Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.PatchAsync(predicate, setItem, validationContext, ct);
    public virtual Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.PatchManyAsync(predicate, setItem, validationContext, ct);
    public virtual Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage.PatchManyAsync(predicate, setItem, validationContext, ct);

    public virtual Task<Result> RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Storage.RemoveAsync(predicate, ct);
    public virtual Task<Result> RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Storage.RemoveManyAsync(predicate, ct);

    public virtual Task<Result> ClearAsync(CancellationToken ct = default)
        => Storage.ClearAsync(ct);
}
