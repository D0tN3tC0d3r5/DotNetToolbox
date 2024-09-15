namespace DotNetToolbox.Data.DataSources;

public class DataSource<TStorage, TItem, TKey>
    : DataSource<TStorage, TItem>
    , IDataSource<TItem, TKey>
    where TStorage : class, IStorage<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    public DataSource(IEnumerable<TItem>? records = null)
        : base(records) {
    }

    public DataSource(TStorage storage)
        : base(storage) {
    }

    #region Blocking

    public TItem? FindByKey(TKey key)
        => Storage is null
               ? throw new NotImplementedException("The method implementation is required when the strategy is not defined.")
               : Storage.FindByKey(key);

    public Result Update(TItem updatedItem, IMap? validationContext = null)
        => Storage?.Update(updatedItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result UpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null)
        => Storage?.UpdateMany(updatedItems, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result AddOrUpdate(TItem updatedItem, IMap? validationContext = null)
        => Storage?.AddOrUpdate(updatedItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result AddOrUpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null)
        => Storage?.AddOrUpdateMany(updatedItems, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result Patch(TKey key, Action<TItem> setItem, IMap? validationContext = null)
        => Storage?.Patch(key, setItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null)
        => Storage?.PatchMany(keys, setItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result Remove(TKey key)
        => Storage?.Remove(key)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result RemoveMany(IEnumerable<TKey> keys)
        => Storage?.RemoveMany(keys)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    #endregion

    #region Async

    public ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => Storage?.FindByKeyAsync(key, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> UpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.UpdateAsync(updatedItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> UpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.UpdateManyAsync(updatedItems, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> AddOrUpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.AddOrUpdateAsync(updatedItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.AddOrUpdateManyAsync(updatedItems, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.PatchAsync(key, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.PatchManyAsync(keys, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.PatchManyAsync(keys, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> RemoveAsync(TKey key, CancellationToken ct = default)
        => Storage?.RemoveAsync(key, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default)
        => Storage?.RemoveManyAsync(keys, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    #endregion
}

public class DataSource<TStorage, TItem>
    : QueryableDataSource<TItem>
    , IDataSource<TItem>
    where TStorage : class, IStorage<TItem> {
    private bool _disposed;

    public DataSource(IEnumerable<TItem>? records = null)
        : base(records) {
    }

    public DataSource(TStorage storage) {
        Storage = IsNotNull(storage);
        Storage.Load();
        Records = Storage.Data;
    }

    public async ValueTask DisposeAsync() {
        if (!_disposed) {
            if (Storage is not null)
                await Storage.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }

    protected TStorage? Storage { get; init; }

    #region Blocking

    public virtual Result Load()
        => Storage?.Load()
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result Seed(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null)
        => Storage?.Seed(seed, preserveContent, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual TItem[] GetAll(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null)
        => Storage?.GetAll(filterBy, orderBy)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null)
        => Storage?.GetPage(pageIndex, pageSize, filterBy, orderBy)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null)
        => Storage?.GetChunk(isChunkStart, blockSize, filterBy, orderBy)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Storage is not null ? Storage.Find(predicate)
        : throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result<TItem> Create(Action<TItem>? setItem = null, IMap? validationContext = null)
        => Storage?.Create(setItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result Add(TItem newItem, IMap? validationContext = null)
        => Storage?.Add(newItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result AddMany(IEnumerable<TItem> newItems, IMap? validationContext = null)
        => Storage?.AddMany(newItems, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null)
        => Storage?.Update(predicate, updatedItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null)
        => Storage?.UpdateMany(predicate, updatedItems, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null)
        => Storage?.AddOrUpdate(predicate, updatedItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items, IMap? validationContext = null)
        => Storage?.AddOrUpdateMany(predicate, items, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null)
        => Storage?.Patch(predicate, setItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null)
        => Storage?.PatchMany(predicate, setItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result Remove(Expression<Func<TItem, bool>> predicate)
        => Storage?.Remove(predicate)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result RemoveMany(Expression<Func<TItem, bool>> predicate)
        => Storage?.RemoveMany(predicate)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result Clear()
        => Storage?.Clear()
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    #endregion

    #region Async

    public virtual Task<Result> SeedAsync(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.SeedAsync(seed, preserveContent, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> LoadAsync(CancellationToken ct = default)
        => Storage?.LoadAsync(ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual ValueTask<TItem[]> GetAllAsync(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default)
        => Storage?.GetAllAsync(filterBy, orderBy, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default)
        => Storage?.GetPageAsync(pageIndex, pageSize, filterBy, orderBy, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default)
        => Storage?.GetChunkAsync(isChunkStart, blockSize, filterBy, orderBy, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Storage?.FindAsync(predicate, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result<TItem>> CreateAsync(Func<TItem, CancellationToken, Task> setItem, IMap validationContext, CancellationToken ct = default)
        => Storage?.CreateAsync(setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => Storage?.CreateAsync(setItem, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> AddAsync(TItem newItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.AddAsync(newItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> AddManyAsync(IEnumerable<TItem> newItems, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.AddManyAsync(newItems, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.UpdateAsync(predicate, updatedItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.AddOrUpdateAsync(predicate, updatedItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.AddOrUpdateManyAsync(predicate, updatedItems, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.PatchAsync(predicate, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.PatchAsync(predicate, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.PatchManyAsync(predicate, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Storage?.PatchManyAsync(predicate, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Storage?.RemoveAsync(predicate, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Storage?.RemoveManyAsync(predicate, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> ClearAsync(CancellationToken ct = default)
        => Storage?.ClearAsync(ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    #endregion
}
