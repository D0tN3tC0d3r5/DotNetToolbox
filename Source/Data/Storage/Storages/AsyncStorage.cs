namespace DotNetToolbox.Data.Storages;

public abstract class AsyncStorage<TStorage, TItem, TKey>(string id, IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? data = null)
    : IAsyncStorage<TItem, TKey>
    where TStorage : AsyncStorage<TStorage, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    private bool _disposedAsync;

    public async ValueTask DisposeAsync() {
        if (_disposedAsync) return;
        await DisposeAsyncCore().ConfigureAwait(false);
        _disposedAsync = true;
        GC.SuppressFinalize(this);
    }
    protected virtual ValueTask DisposeAsyncCore() => ValueTask.CompletedTask;

    public string Id { get; init; } = id;
    public IAsyncKeyGenerator<TKey> KeyGenerator { get; init; } = keyGenerator;
    public List<TItem> Data { get; init; } = data as List<TItem> ?? [.. data ?? []];
    protected Task<TKey> GetNextKeyAsync(CancellationToken ct = default) => KeyGenerator.GenerateNextKeyAsync(Id, ct);
    public virtual Task<Result> LoadAsync(CancellationToken ct = default) => KeyGenerator.InitializeAsync(Id, ct);

    public abstract ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default);
    public abstract Task<Result> UpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> UpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> AddOrUpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> RemoveAsync(TKey key, CancellationToken ct = default);
    public abstract Task<Result> RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default);
    public abstract Task<Result> SeedAsync(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null, CancellationToken ct = default);
    public abstract ValueTask<TItem[]> GetAllAsync(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default);
    public abstract ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default);
    public abstract ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = 20U, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default);
    public abstract ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    public abstract Task<Result<TItem>> CreateAsync(Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> AddAsync(TItem newItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> AddManyAsync(IEnumerable<TItem> newItems, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default);
    public abstract Task<Result> RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    public abstract Task<Result> RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    public abstract Task<Result> ClearAsync(CancellationToken ct = default);
}
