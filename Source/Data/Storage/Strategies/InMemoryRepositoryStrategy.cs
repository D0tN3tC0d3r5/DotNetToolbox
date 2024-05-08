namespace DotNetToolbox.Data.Strategies;

public class InMemoryRepositoryStrategy<TItem, TKey>
    : RepositoryStrategy<InMemoryRepositoryStrategy<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    private readonly IRepositoryStrategy<TItem> _keylessStrategy = new InMemoryRepositoryStrategy<TItem>();

    public override void SetRepository(IQueryableRepository repository) {
        base.SetRepository(repository);
        _keylessStrategy.SetRepository(repository);
        SetKeyHandler(InMemoryKeyHandlerFactory.CreateDefault<TKey>(Repository.Id));
    }

    #region Blocking

    public override void Seed(IEnumerable<TItem> seed)
        => _keylessStrategy.Seed(seed);
    public override void Load()
        => _keylessStrategy.Load();

    public override TItem[] GetAll()
        => _keylessStrategy.GetAll();
    public override Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = 20)
        => _keylessStrategy.GetPage(pageIndex, pageSize);
    public override Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = 20)
        => _keylessStrategy.GetChunk(isChunkStart, blockSize);

    public override TItem? Find(Expression<Func<TItem, bool>> predicate)
        => _keylessStrategy.Find(predicate);
    public override TItem? FindByKey(TKey key)
        => Find(x => KeyHandler.Equals(x.Key, key));

    public override TItem Create(Action<TItem> setItem) {
        var item = _keylessStrategy.Create(setItem);
        item.Key = KeyHandler.GetNext(item.Key);
        return item;
    }

    public override void Add(TItem newItem) {
        newItem.Key = KeyHandler.GetNext(newItem.Key);
        _keylessStrategy.Add(newItem);
    }

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => _keylessStrategy.Update(predicate, updatedItem);
    public override void Update(TItem updatedItem)
        => Update(x => KeyHandler.Equals(x.Key, updatedItem.Key), updatedItem);

    public override void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem)
        => _keylessStrategy.Patch(predicate, setItem);
    public override void Patch(TKey key, Action<TItem> setItem)
        => Patch(x => KeyHandler.Equals(x.Key, key), setItem);

    public override void Remove(Expression<Func<TItem, bool>> predicate)
        => _keylessStrategy.Remove(predicate);
    public override void Remove(TKey key)
        => Remove(x => KeyHandler.Equals(x.Key, key));

    public override void AddMany(IEnumerable<TItem> newItems)
        => _keylessStrategy.AddMany(newItems);
    public override void UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems)
        => _keylessStrategy.UpdateMany(predicate, updatedItems);
    public override void AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => _keylessStrategy.AddOrUpdate(predicate, updatedItem);
    public override void AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items)
        => _keylessStrategy.AddOrUpdateMany(predicate, items);
    public override void PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem)
        => _keylessStrategy.PatchMany(predicate, setItem);
    public override void RemoveMany(Expression<Func<TItem, bool>> predicate)
        => _keylessStrategy.RemoveMany(predicate);
    public override void Clear()
        => _keylessStrategy.Clear();

    public override void UpdateMany(IEnumerable<TItem> updatedItems) {
        foreach (var updatedItem in updatedItems)
            Update(updatedItem);
    }

    public override void AddOrUpdate(TItem updatedItem) {
        Remove(updatedItem.Key);
        _keylessStrategy.Add(updatedItem);
    }

    public override void AddOrUpdateMany(IEnumerable<TItem> updatedItems) {
        foreach (var item in updatedItems)
            AddOrUpdate(item);
    }

    public override void PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem) {
        foreach (var key in keys) {
            var item = FindByKey(key);
            if (item is not null) setItem(item);
        }
    }

    public override void RemoveMany(IEnumerable<TKey> keys) {
        foreach (var key in keys)
            Remove(key);
    }

    #endregion

    #region Async

    public override Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default)
        => _keylessStrategy.SeedAsync(seed, ct);
    public override Task LoadAsync(CancellationToken ct = default)
        => _keylessStrategy.LoadAsync(ct);

    public override ValueTask<TItem[]> GetAllAsync(CancellationToken ct = default)
        => _keylessStrategy.GetAllAsync(ct);
    public override ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = 20, CancellationToken ct = default)
        => _keylessStrategy.GetPageAsync(pageIndex, pageSize, ct);
    public override ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = 20, CancellationToken ct = default)
        => _keylessStrategy.GetChunkAsync(isChunkStart, blockSize, ct);

    public override ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _keylessStrategy.FindAsync(predicate, ct);
    public override ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => FindAsync(x => KeyHandler.Equals(x.Key, key), ct);

    public override async Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) {
        var item = await _keylessStrategy.CreateAsync(setItem, ct);
        item.Key = await KeyHandler.GetNextAsync(item.Key, ct);
        return item;
    }

    public override async Task AddAsync(TItem newItem, CancellationToken ct = default) {
        newItem.Key = await KeyHandler.GetNextAsync(newItem.Key, ct);
        await _keylessStrategy.AddAsync(newItem, ct);
    }

    public override Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => _keylessStrategy.UpdateAsync(predicate, updatedItem, ct);
    public override Task UpdateAsync(TItem updatedItem, CancellationToken ct = default)
        => UpdateAsync(x => KeyHandler.Equals(x.Key, updatedItem.Key), updatedItem, ct);

    public override Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => _keylessStrategy.PatchAsync(predicate, setItem, ct);
    public override Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => PatchAsync(x => KeyHandler.Equals(x.Key, key), setItem, ct);

    public override Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _keylessStrategy.RemoveAsync(predicate, ct);
    public override Task RemoveAsync(TKey key, CancellationToken ct = default)
        => RemoveAsync(x => KeyHandler.Equals(x.Key, key), ct);

    public override Task AddManyAsync(IEnumerable<TItem> newItems, CancellationToken ct = default)
        => _keylessStrategy.AddManyAsync(newItems, ct);
    public override Task AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => _keylessStrategy.AddOrUpdateAsync(predicate, updatedItem, ct);
    public override Task AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, CancellationToken ct = default)
        => _keylessStrategy.AddOrUpdateManyAsync(predicate, updatedItems, ct);
    public override Task PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, CancellationToken ct = default)
        => _keylessStrategy.PatchAsync(predicate, setItem, ct);
    public override Task PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, CancellationToken ct = default)
        => _keylessStrategy.PatchManyAsync(predicate, setItem, ct);
    public override Task PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => _keylessStrategy.PatchManyAsync(predicate, setItem, ct);
    public override Task RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _keylessStrategy.RemoveManyAsync(predicate, ct);
    public override Task ClearAsync(CancellationToken ct = default)
        => _keylessStrategy.ClearAsync(ct);

    public override async Task UpdateManyAsync(IEnumerable<TItem> updatedItems, CancellationToken ct = default) {
        await foreach (var updatedItem in updatedItems.AsAsyncEnumerable(ct))
            await UpdateAsync(updatedItem, ct);
    }

    public override async Task AddOrUpdateAsync(TItem updatedItem, CancellationToken ct = default) {
        await RemoveAsync(updatedItem.Key, ct);
        await AddAsync(updatedItem, ct);
    }

    public override async Task AddOrUpdateManyAsync(IEnumerable<TItem> updatedItems, CancellationToken ct = default) {
        await foreach (var item in updatedItems.AsAsyncEnumerable(ct)) {
            await RemoveAsync(item.Key, ct);
            await AddAsync(item, ct);
        }
    }

    public override async Task PatchManyAsync(IEnumerable<TKey> keys, Action<TItem> setItem, CancellationToken ct = default) {
        await foreach (var key in keys.AsAsyncEnumerable(ct)) {
            var item = await FindByKeyAsync(key, ct);
            if (item != null) setItem(item);
        }
    }
    public override async Task PatchManyAsync(IEnumerable<TKey> keys, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) {
        await foreach (var key in keys.AsAsyncEnumerable(ct)) {
            var item = await FindByKeyAsync(key, ct);
            if (item != null) await setItem(item, ct);
        }
    }

    public override async Task RemoveManyAsync(IEnumerable<TKey> keys, CancellationToken ct = default) {
        await foreach (var item in Repository.Query.Where(i => keys.Any(k => KeyHandler.Equals(k, i.Key))).AsAsyncEnumerable(ct))
            Repository.Data.Remove(item);
    }

    #endregion
}

public class InMemoryRepositoryStrategy<TItem>
    : RepositoryStrategy<InMemoryRepositoryStrategy<TItem>, TItem> {
    #region Blocking

    public override void Seed(IEnumerable<TItem> seed)
        => Repository.Data = seed as List<TItem> ?? IsNotNull(seed).ToList();
    public override void Load() { }

    public override TItem[] GetAll()
        => Repository.Query.ToArray();

    public override Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize) {
        var count = Repository.Query.Count();
        var items = Repository.Query.Skip((int)(pageIndex * pageSize))
                   .Take((int)pageSize)
                   .ToArray();
        return new() {
            TotalCount = (uint)count,
            Index = pageIndex,
            Size = pageSize,
            Items = items,
        };
    }

    public override Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize) {
        var query = Repository.Query;
        if (isChunkStart is not null) {
            var isNotStart = (Expression<Func<TItem, bool>>)Expression.Lambda(Expression.Not(isChunkStart.Body), isChunkStart.Parameters);
            query = query.SkipWhile(isNotStart);
        }
        var items = query
                   .Take((int)blockSize)
                   .ToArray();
        return new() {
            Size = blockSize,
            Items = items,
        };
    }

    public override TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Repository.Query.FirstOrDefault(predicate);

    public override TItem Create(Action<TItem> setItem) {
        var item = Activator.CreateInstance<TItem>();
        setItem(item);
        return item;
    }

    public override void Add(TItem newItem)
        => Repository.Data.Add(newItem);
    public override void AddMany(IEnumerable<TItem> newItems)
        => Repository.Data.AddRange(newItems);

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) {
        if (!TryRemove(predicate)) return;
        Add(updatedItem);
    }
    public override void AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem item) {
        Remove(predicate);
        Add(item);
    }
    public override void AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items) {
        Remove(predicate);
        AddMany(items);
    }

    public override void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem) {
        var itemToPatch = Repository.Query.FirstOrDefault(predicate);
        if (itemToPatch is null) return;
        setItem(itemToPatch);
    }
    public override void PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem) {
        var itemsToPatch = Repository.Query.Where(predicate);
        foreach (var item in itemsToPatch)
            setItem(item);
    }

    public override void Remove(Expression<Func<TItem, bool>> predicate)
        => TryRemove(predicate);
    public override void RemoveMany(Expression<Func<TItem, bool>> predicate) {
        var itemsToRemove = Repository.Query.Where(predicate);
        foreach (var item in itemsToRemove) Repository.Data.Remove(item);
    }

    public override void Clear()
        => Repository.Data.Clear();

    private bool TryRemove(Expression<Func<TItem, bool>> predicate) {
        var itemToRemove = Repository.Query.FirstOrDefault(predicate);
        if (itemToRemove is null)
            return false;
        Repository.Data.Remove(itemToRemove);
        return true;
    }

#endregion

    #region Async

    public override Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default) {
        Seed(seed);
        return Task.CompletedTask;
    }

    public override Task LoadAsync(CancellationToken ct = default) {
        Load();
        return Task.CompletedTask;
    }

    public override ValueTask<TItem[]> GetAllAsync(CancellationToken ct = default)
        => Repository.AsyncQuery.ToArrayAsync(ct);

    public override async ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, CancellationToken ct = default) {
        var count = await Repository.AsyncQuery.CountAsync(ct);
        var items = await Repository.AsyncQuery.Skip((int)(pageIndex * pageSize))
                   .Take((int)pageSize)
                   .ToArrayAsync(ct);
        return new() {
            TotalCount = (uint)count,
            Index = pageIndex,
            Size = pageSize,
            Items = items,
        };
    }

    public override async ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, CancellationToken ct = default) {
        var query = Repository.AsyncQuery;
        if (isChunkStart is not null) {
            var isNotStart = (Expression<Func<TItem, bool>>)Expression.Lambda(Expression.Not(isChunkStart.Body), isChunkStart.Parameters);
            query = query.SkipWhile(isNotStart).AsAsyncQueryable();
        }
        var items = await query
                   .Take((int)blockSize)
                   .ToArrayAsync(ct);
        return new() {
            Size = blockSize,
            Items = items,
        };
    }

    public override ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Repository.Query.FirstOrDefaultAsync(predicate, ct);

    public override async Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) {
        var item = Activator.CreateInstance<TItem>();
        await setItem(item, ct);
        return item;
    }

    public override Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Task.Run(() => Repository.Data.Add(newItem), ct);
    public override async Task AddManyAsync(IEnumerable<TItem> newItems, CancellationToken ct = default) {
        await foreach (var item in newItems.AsAsyncEnumerable(ct))
            Repository.Data.Add(item);
    }

    public override async Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) {
        if (!await TryRemoveAsync(predicate, ct)) return;
        await AddAsync(updatedItem, ct);
    }

    public override async Task AddOrUpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) {
        await RemoveAsync(predicate, ct);
        await AddAsync(updatedItem, ct);
    }
    public override async Task AddOrUpdateManyAsync(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, CancellationToken ct = default) {
        await RemoveAsync(predicate, ct);
        await AddManyAsync(updatedItems, ct);
    }

    public override async Task PatchAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, CancellationToken ct = default) {
        var itemToPatch = await Repository.AsyncQuery.FirstOrDefaultAsync(predicate, ct);
        if (itemToPatch is null)
            return;
        setItem(itemToPatch);
    }
    public override async Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) {
        var itemToPatch = await Repository.AsyncQuery.FirstOrDefaultAsync(predicate, ct);
        if (itemToPatch is null) return;
        await setItem(itemToPatch, ct);
    }

    public override async Task PatchManyAsync(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, CancellationToken ct = default) {
        await foreach (var item in Repository.Query.Where(predicate).AsAsyncEnumerable(ct))
            setItem(item);
    }
    public override async Task PatchManyAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) {
        await foreach (var item in Repository.Query.Where(predicate).AsAsyncEnumerable(ct))
            await setItem(item, ct);
    }

    public override Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => TryRemoveAsync(predicate, ct);
    public override async Task RemoveManyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        await foreach (var item in Repository.Query.Where(predicate).AsAsyncEnumerable(ct))
            Repository.Data.Remove(item);
    }

    public override Task ClearAsync(CancellationToken ct = default)
        => Task.Run(Repository.Data.Clear, ct);

    private async Task<bool> TryRemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        var itemToRemove = await Repository.AsyncQuery.FirstOrDefaultAsync(predicate, ct);
        if (itemToRemove is null)
            return false;
        Repository.Data.Remove(itemToRemove);
        return true;
    }

    #endregion
}
