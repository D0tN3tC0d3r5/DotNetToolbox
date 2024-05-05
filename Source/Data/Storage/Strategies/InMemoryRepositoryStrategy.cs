namespace DotNetToolbox.Data.Strategies;

public class InMemoryRepositoryStrategy<TItem, TKey>
    : RepositoryStrategy<InMemoryRepositoryStrategy<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    private readonly IRepositoryStrategy<TItem> _keylessStrategy = new InMemoryRepositoryStrategy<TItem>();

    public override void SetRepository(IRepositoryBase repository) {
        base.SetRepository(repository);
        _keylessStrategy.SetRepository(repository);
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
    public override Chunk<TItem> GetFirstChunk(uint blockSize = 20)
        => _keylessStrategy.GetFirstChunk(blockSize);
    public override Chunk<TItem> GetNextChunk(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = 20)
        => _keylessStrategy.GetNextChunk(isChunkStart, blockSize);

    public override TItem? Find(Expression<Func<TItem, bool>> predicate)
        => _keylessStrategy.Find(predicate);
    public override TItem? FindByKey(TKey key)
        => Find(x => KeyHandler.Equals(x.Key, key));

    public override TItem Create(Action<TItem> setItem) {
        var item = _keylessStrategy.Create(setItem);
        item.Key = KeyHandler.GetNext(Repository.Name, item.Key);
        return item;
    }

    public override void Add(TItem newItem) {
        newItem.Key = KeyHandler.GetNext(Repository.Name, newItem.Key);
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
    public override ValueTask<Chunk<TItem>> GetFirstChunkAsync(uint blockSize = 20, CancellationToken ct = default)
        => _keylessStrategy.GetFirstChunkAsync(blockSize, ct);
    public override ValueTask<Chunk<TItem>> GetNextChunkAsync(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = 20, CancellationToken ct = default)
        => _keylessStrategy.GetNextChunkAsync(isChunkStart, blockSize, ct);

    public override ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _keylessStrategy.FindAsync(predicate, ct);
    public override ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => FindAsync(x => KeyHandler.Equals(x.Key, key), ct);

    public override async Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) {
        var item = await _keylessStrategy.CreateAsync(setItem, ct);
        item.Key = await KeyHandler.GetNextAsync(Repository.Name, item.Key, ct);
        return item;
    }

    public override async Task AddAsync(TItem newItem, CancellationToken ct = default) {
        newItem.Key = await KeyHandler.GetNextAsync(Repository.Name, newItem.Key, ct);
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

    public override Chunk<TItem> GetFirstChunk(uint blockSize = DefaultBlockSize)
        => GetNextChunk(_ => true, blockSize);

    public override Chunk<TItem> GetNextChunk(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = DefaultBlockSize) {
        var isNotStart = (Expression<Func<TItem, bool>>)Expression.Lambda(Expression.Not(isChunkStart.Body), isChunkStart.Parameters);
        var items = Repository.Query.SkipWhile(isNotStart)
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

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) {
        if (!TryRemove(predicate))
            return;
        Add(updatedItem);
    }

    public override void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem) {
        var itemToPatch = Repository.Query.FirstOrDefault(predicate);
        if (itemToPatch is null)
            return;
        setItem(itemToPatch);
    }

    public override void Remove(Expression<Func<TItem, bool>> predicate)
        => TryRemove(predicate);

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

    public override ValueTask<Chunk<TItem>> GetFirstChunkAsync(uint blockSize = DefaultBlockSize, CancellationToken ct = default)
        => GetNextChunkAsync(_ => true, blockSize, ct);

    public override async ValueTask<Chunk<TItem>> GetNextChunkAsync(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = DefaultBlockSize, CancellationToken ct = default) {
        var isNotStart = (Expression<Func<TItem, bool>>)Expression.Lambda(Expression.Not(isChunkStart.Body), isChunkStart.Parameters);
        var items = await Repository.AsyncQuery.SkipWhile(isNotStart)
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

    public override Task AddAsync(TItem newItem, CancellationToken ct = default) {
        Repository.Data.Add(newItem);
        return Task.CompletedTask;
    }

    public override async Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) {
        if (!await TryRemoveAsync(predicate, ct))
            return;
        await AddAsync(updatedItem, ct);
    }

    public override async Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) {
        var itemToPatch = await Repository.AsyncQuery.FirstOrDefaultAsync(predicate, ct);
        if (itemToPatch is null)
            return;
        await setItem(itemToPatch, ct);
    }

    public override Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => TryRemoveAsync(predicate, ct);

    private async Task<bool> TryRemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        var itemToRemove = await Repository.AsyncQuery.FirstOrDefaultAsync(predicate, ct);
        if (itemToRemove is null)
            return false;
        Repository.Data.Remove(itemToRemove);
        return true;
    }

    #endregion
}
