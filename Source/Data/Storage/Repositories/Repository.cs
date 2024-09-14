namespace DotNetToolbox.Data.Repositories;

public class Repository<TStrategy, TItem, TKey>
    : Repository<TStrategy, TItem>
    , IRepository<TItem, TKey>
    where TStrategy : class, IRepositoryStrategy<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    public Repository(IEnumerable<TItem>? data = null)
        : base(data) {
    }

    public Repository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }

    #region Blocking

    public TItem? FindByKey(TKey key)
        => Strategy is null
               ? throw new NotImplementedException("The method implementation is required when the strategy is not defined.")
               : Strategy.FindByKey(key);

    public Result Update(TItem updatedItem, IMap? validationContext = null)
        => Strategy?.Update(updatedItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result UpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null)
        => Strategy?.UpdateMany(updatedItems, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result AddOrUpdate(TItem updatedItem, IMap? validationContext = null)
        => Strategy?.AddOrUpdate(updatedItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result AddOrUpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null)
        => Strategy?.AddOrUpdateMany(updatedItems, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result Patch(TKey key, Action<TItem> setItem, IMap? validationContext = null)
        => Strategy?.Patch(key, setItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null)
        => Strategy?.PatchMany(keys, setItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result Remove(TKey key)
        => Strategy?.Remove(key)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Result RemoveMany(IEnumerable<TKey> keys)
        => Strategy?.RemoveMany(keys)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    #endregion

    #region Async

    public ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => Strategy?.FindByKeyAsync(key, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> UpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.UpdateAsync(updatedItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> UpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.UpdateManyAsync(updatedItems, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> AddOrUpdateAsync(TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.AddOrUpdateAsync(updatedItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.AddOrUpdateManyAsync(updatedItems, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.PatchAsync(key, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.PatchManyAsync(keys, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public Task<Result> PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.PatchManyAsync(keys, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> RemoveAsync(TKey key, CancellationToken ct = default)
        => Strategy?.RemoveAsync(key, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public Task<Result> RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default)
        => Strategy?.RemoveManyAsync(keys, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    #endregion
}

public class Repository<TStrategy, TItem>
    : QueryableRepository<TItem>
    , IRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {
    private bool _disposed;

    public Repository(IEnumerable<TItem>? data = null)
        : base(data) {
    }

    public Repository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(data) {
        Strategy = IsNotNull(strategy);
        Strategy.Repository = this;
        Strategy.Load();
    }

    public async ValueTask DisposeAsync() {
        if (!_disposed) {
            if (Strategy is not null)
                await Strategy.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }

    protected TStrategy? Strategy { get; init; }

    #region Blocking

    public virtual Result Load()
        => Strategy?.Load()
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result Seed(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null)
        => Strategy?.Seed(seed, preserveContent, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual TItem[] GetAll(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null)
        => Strategy?.GetAll(filterBy, orderBy)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null)
        => Strategy?.GetPage(pageIndex, pageSize, filterBy, orderBy)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null)
        => Strategy?.GetChunk(isChunkStart, blockSize, filterBy, orderBy)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Strategy is not null ? Strategy.Find(predicate)
        : throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result<TItem> Create(Action<TItem>? setItem = null, IMap? validationContext = null)
        => Strategy?.Create(setItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result Add(TItem newItem, IMap? validationContext = null)
        => Strategy?.Add(newItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result AddMany(IEnumerable<TItem> newItems, IMap? validationContext = null)
        => Strategy?.AddMany(newItems, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null)
        => Strategy?.Update(predicate, updatedItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null)
        => Strategy?.UpdateMany(predicate, updatedItems, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null)
        => Strategy?.AddOrUpdate(predicate, updatedItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items, IMap? validationContext = null)
        => Strategy?.AddOrUpdateMany(predicate, items, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null)
        => Strategy?.Patch(predicate, setItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null)
        => Strategy?.PatchMany(predicate, setItem, validationContext)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result Remove(Expression<Func<TItem, bool>> predicate)
        => Strategy?.Remove(predicate)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Result RemoveMany(Expression<Func<TItem, bool>> predicate)
        => Strategy?.RemoveMany(predicate)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Result Clear()
        => Strategy?.Clear()
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    #endregion

    #region Async

    public virtual Task<Result> SeedAsync(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.SeedAsync(seed, preserveContent, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> LoadAsync(CancellationToken ct = default)
        => Strategy?.LoadAsync(ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual ValueTask<TItem[]> GetAllAsync(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default)
        => Strategy?.GetAllAsync(filterBy, orderBy, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default)
        => Strategy?.GetPageAsync(pageIndex, pageSize, filterBy, orderBy, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default)
        => Strategy?.GetChunkAsync(isChunkStart, blockSize, filterBy, orderBy, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy?.FindAsync(predicate, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result<TItem>> CreateAsync(Func<TItem, CancellationToken, Task> setItem, IMap validationContext, CancellationToken ct = default)
        => Strategy?.CreateAsync(setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => Strategy?.CreateAsync(setItem, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> AddAsync(TItem newItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.AddAsync(newItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> AddManyAsync(IEnumerable<TItem> newItems, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.AddManyAsync(newItems, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.UpdateAsync(predicate, updatedItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.AddOrUpdateAsync(predicate, updatedItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.AddOrUpdateManyAsync(predicate, updatedItems, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.PatchAsync(predicate, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.PatchAsync(predicate, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.PatchManyAsync(predicate, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default)
        => Strategy?.PatchManyAsync(predicate, setItem, validationContext, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy?.RemoveAsync(predicate, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");
    public virtual Task<Result> RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy?.RemoveManyAsync(predicate, ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    public virtual Task<Result> ClearAsync(CancellationToken ct = default)
        => Strategy?.ClearAsync(ct)
        ?? throw new NotImplementedException("The method implementation is required when the strategy is not defined.");

    #endregion
}
