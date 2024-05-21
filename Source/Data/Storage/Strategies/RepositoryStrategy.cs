namespace DotNetToolbox.Data.Strategies;

public class RepositoryStrategy<TStrategy, TItem, TKey>
    : RepositoryStrategy<TStrategy, TItem>
    , IRepositoryStrategy<TItem, TKey>
    where TStrategy : RepositoryStrategy<TStrategy, TItem, TKey>, new()
    where TItem : IEntity<TKey>
    where TKey : notnull {

    protected IKeyHandler<TKey> KeyHandler { get; private set; } = NullKeyHandler<TKey>.Default;
    public void SetKeyHandler(IKeyHandler<TKey> keyHandler)
        => KeyHandler = keyHandler;

    #region Blocking

    public virtual TItem? FindByKey(TKey key)
        => throw new NotImplementedException();

    public virtual void Update(TItem updatedItem)
        => throw new NotImplementedException();
    public virtual void UpdateMany(IEnumerable<TItem> updatedItems)
        => throw new NotImplementedException();

    public virtual void AddOrUpdate(TItem updatedItem)
        => throw new NotImplementedException();

    public virtual void AddOrUpdateMany(IEnumerable<TItem> updatedItems)
        => throw new NotImplementedException();

    public virtual void Patch(TKey key, Action<TItem> setItem)
        => throw new NotImplementedException();
    public virtual void PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem)
        => throw new NotImplementedException();

    public virtual void Remove(TKey key)
        => throw new NotImplementedException();
    public virtual void RemoveMany(IEnumerable<TKey> keys)
        => throw new NotImplementedException();

#endregion

    #region Async
    public virtual ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task UpdateAsync(TItem updatedItem, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task UpdateManyAsync(IEnumerable<TItem> updatedItems, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task AddOrUpdateAsync(TItem updatedItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task RemoveAsync(TKey key, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default)
        => throw new NotImplementedException();

#endregion
}

public class RepositoryStrategy<TStrategy, TItem>
    : IRepositoryStrategy<TItem>
    where TStrategy : RepositoryStrategy<TStrategy, TItem>, new() {
    protected QueryableRepository<TItem> Repository { get; private set; } = default!;

    public virtual void SetRepository(IQueryableRepository repository)
        => Repository = IsOfType<QueryableRepository<TItem>>(repository);

    #region Blocking

    public virtual void Seed(IEnumerable<TItem> seed)
        => throw new NotImplementedException();
    public virtual void Load()
        => throw new NotImplementedException();

    public virtual TItem[] GetAll()
        => throw new NotImplementedException();
    public virtual Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize)
        => throw new NotImplementedException();
    public virtual Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize)
        => throw new NotImplementedException();

    public virtual TItem? Find(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem Create(Action<TItem> setItem)
        => throw new NotImplementedException();
    public virtual void Add(TItem newItem)
        => throw new NotImplementedException();

    public virtual void AddMany(IEnumerable<TItem> newItems)
        => throw new NotImplementedException();

    public virtual void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => throw new NotImplementedException();

    public virtual void UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems)
        => throw new NotImplementedException();

    public virtual void AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => throw new NotImplementedException();

    public virtual void AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items)
        => throw new NotImplementedException();

    public virtual void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem)
        => throw new NotImplementedException();

    public virtual void PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem)
        => throw new NotImplementedException();

    public virtual void Remove(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual void RemoveMany(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual void Clear()
        => throw new NotImplementedException();

#endregion

    #region Async

    public virtual Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task LoadAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual ValueTask<TItem[]> GetAllAsync(CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = 20U, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task AddAsync(TItem newItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task AddManyAsync(IEnumerable<TItem> newItems, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task ClearAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

#endregion
}
