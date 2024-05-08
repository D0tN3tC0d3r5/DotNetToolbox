namespace DotNetToolbox.Data.Repositories;

public class Repository<TStrategy, TItem, TKey>
    : Repository<TStrategy, TItem>
    , IRepository<TItem, TKey>
    where TStrategy : class, IRepositoryStrategy<TItem, TKey>, new()
    where TItem : IEntity<TKey>
    where TKey : notnull {

    public void SetKeyHandler(IKeyHandler<TKey> keyHandler)
        => Strategy.SetKeyHandler(keyHandler);

#region Blocking

    public TItem? FindByKey(TKey key)
        => Strategy.FindByKey(key);
    public void Update(TItem updatedItem)
        => Strategy.Update(updatedItem);

    public void UpdateMany(IEnumerable<TItem> updatedItems)
         => Strategy.UpdateMany(updatedItems);

    public void AddOrUpdate(TItem updatedItem)
         => Strategy.AddOrUpdate(updatedItem);

    public void AddOrUpdateMany(IEnumerable<TItem> updatedItems)
         => Strategy.AddOrUpdateMany(updatedItems);

    public void Patch(TKey key, Action<TItem> setItem)
        => Strategy.Patch(key, setItem);

    public void PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem)
         => Strategy.PatchMany(keys, setItem);

    public void Remove(TKey key)
        => Strategy.Remove(key);

    public void RemoveMany(IEnumerable<TKey> keys)
         => Strategy.RemoveMany(keys);

#endregion

#region Async

    public ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => Strategy.FindByKeyAsync(key, ct);
    public Task UpdateAsync(TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(updatedItem, ct);

    public Task UpdateManyAsync(IEnumerable<TItem> updatedItems, CancellationToken ct = default)
         => Strategy.UpdateManyAsync(updatedItems, ct);

    public Task AddOrUpdateAsync(TItem updatedItem, CancellationToken ct = default)
         => Strategy.AddOrUpdateAsync(updatedItem, ct);

    public Task AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, CancellationToken ct = default)
         => Strategy.AddOrUpdateManyAsync(updatedItems, ct);

    public Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => Strategy.PatchAsync(key, setItem, ct);

    public Task PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, CancellationToken ct = default)
         => Strategy.PatchManyAsync(keys, setItem, ct);
    public Task PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
         => Strategy.PatchManyAsync(keys, setItem, ct);

    public Task RemoveAsync(TKey key, CancellationToken ct = default)
        => Strategy.RemoveAsync(key, ct);

    public Task RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default)
         => Strategy.RemoveManyAsync(keys, ct);

#endregion
}

public class Repository<TStrategy, TItem>
    : QueryableRepository<TItem>
    , IRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem>, new() {

    public Repository() {
        Strategy = new();
        Strategy.SetRepository(this);
    }

    protected TStrategy Strategy { get; }

#region Blocking

    public void Load()
        => Strategy.Load();
    public void Seed(IEnumerable<TItem> seed)
        => Strategy.Seed(seed);

    public TItem[] GetAll()
        => Strategy.GetAll();

    public Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize)
        => Strategy.GetPage(pageIndex, pageSize);

    public Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize)
        => Strategy.GetChunk(isChunkStart, blockSize);

    public TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Strategy.Find(predicate);

    public TItem Create(Action<TItem> setItem)
        => Strategy.Create(setItem);
    public void Add(TItem newItem)
        => Strategy.Add(newItem);

    public void AddMany(IEnumerable<TItem> newItems)
         => Strategy.AddMany(newItems);

    public void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => Strategy.Update(predicate, updatedItem);
    public void UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems)
         => Strategy.UpdateMany(predicate, updatedItems);

    public void AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
         => Strategy.AddOrUpdate(predicate, updatedItem);
    public void AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items)
         => Strategy.AddOrUpdateMany(predicate, items);

    public void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem)
        => Strategy.Patch(predicate, setItem);
    public void PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem)
         => Strategy.PatchMany(predicate, setItem);

    public void Remove(Expression<Func<TItem, bool>> predicate)
        => Strategy.Remove(predicate);
    public void RemoveMany(Expression<Func<TItem, bool>> predicate)
         => Strategy.RemoveMany(predicate);

    public void Clear()
         => Strategy.Clear();

#endregion

#region Async

    public Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default)
        => Strategy.SeedAsync(seed, ct);
    public Task LoadAsync(CancellationToken ct = default)
        => Strategy.LoadAsync(ct);

    public ValueTask<TItem[]> GetAllAsync(CancellationToken ct = default)
        => Strategy.GetAllAsync(ct);

    public ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, CancellationToken ct = default)
        => Strategy.GetPageAsync(pageIndex, pageSize, ct);
    public ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, CancellationToken ct = default)
        => Strategy.GetChunkAsync(isChunkStart, blockSize, ct);

    public ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.FindAsync(predicate, ct);

    public Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => Strategy.CreateAsync(setItem, ct);

    public Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Strategy.AddAsync(newItem, ct);
    public Task AddManyAsync(IEnumerable<TItem> newItems, CancellationToken ct = default)
         => Strategy.AddManyAsync(newItems, ct);

    public Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(predicate, updatedItem, ct);

    public Task AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
         => Strategy.AddOrUpdateAsync(predicate, updatedItem, ct);
    public Task AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, CancellationToken ct = default)
         => Strategy.AddOrUpdateManyAsync(predicate, updatedItems, ct);

    public Task PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, CancellationToken ct = default)
         => Strategy.PatchAsync(predicate, setItem, ct);
    public Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => Strategy.PatchAsync(predicate, setItem, ct);
    public Task PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, CancellationToken ct = default)
         => Strategy.PatchManyAsync(predicate, setItem, ct);
    public Task PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
         => Strategy.PatchManyAsync(predicate, setItem, ct);

    public Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.RemoveAsync(predicate, ct);
    public Task RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
         => Strategy.RemoveManyAsync(predicate, ct);

    public Task ClearAsync(CancellationToken ct = default)
         => Strategy.ClearAsync(ct);

#endregion
}
