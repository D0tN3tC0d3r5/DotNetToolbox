namespace DotNetToolbox.Data.Strategies;

public class RepositoryStrategy<TStrategy, TItem, TKey>
    : RepositoryStrategy<TStrategy, TItem>
    , IRepositoryStrategy<TItem, TKey>
    where TStrategy : RepositoryStrategy<TStrategy, TItem, TKey>, new()
    where TItem : IEntity<TKey>
    where TKey : notnull {

    protected IKeyHandler<TKey> KeyHandler { get; private set; } = KeyHandler<TKey>.Default;
    public void SetKeyHandler(IKeyHandler<TKey> keyHandler)
        => KeyHandler = keyHandler;

    #region Blocking

    public virtual TItem? FindByKey(TKey key) => throw new NotImplementedException();

    public virtual void Update(TItem updatedItem) => throw new NotImplementedException();
    public virtual void Patch(TKey key, Action<TItem> setItem) => throw new NotImplementedException();
    public virtual void Remove(TKey key) => throw new NotImplementedException();

    #endregion

    #region Async
    public virtual ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task UpdateAsync(TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task RemoveAsync(TKey key, CancellationToken ct = default) => throw new NotImplementedException();

    #endregion
}

public class RepositoryStrategy<TStrategy, TItem>
    : IRepositoryStrategy<TItem>
    where TStrategy : RepositoryStrategy<TStrategy, TItem>, new() {
    protected RepositoryBase<TItem> Repository { get; private set; } = default!;

    public virtual void SetRepository(IRepositoryBase repository)
        => Repository = IsOfType<RepositoryBase<TItem>>(repository);

    #region Blocking

    public virtual void Seed(IEnumerable<TItem> seed)
        => throw new NotImplementedException();
    public virtual void Load()
        => throw new NotImplementedException();

    public virtual TItem[] GetAll()
        => throw new NotImplementedException();
    public virtual Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize)
        => throw new NotImplementedException();
    public virtual Chunk<TItem> GetFirstChunk(uint blockSize = DefaultBlockSize)
        => throw new NotImplementedException();
    public virtual Chunk<TItem> GetNextChunk(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = DefaultBlockSize)
        => throw new NotImplementedException();

    public virtual TItem? Find(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem Create(Action<TItem> setItem)
        => throw new NotImplementedException();
    public virtual void Add(TItem newItem)
        => throw new NotImplementedException();

    public virtual void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => throw new NotImplementedException();
    public virtual void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem)
        => throw new NotImplementedException();

    public virtual void Remove(Expression<Func<TItem, bool>> predicate)
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
    public virtual ValueTask<Chunk<TItem>> GetFirstChunkAsync(uint blockSize = 20U, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual ValueTask<Chunk<TItem>> GetNextChunkAsync(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = 20U, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task AddAsync(TItem newItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    #endregion
}
