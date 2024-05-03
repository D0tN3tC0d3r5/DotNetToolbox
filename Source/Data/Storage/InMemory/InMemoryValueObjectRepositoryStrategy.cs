namespace DotNetToolbox.Data.InMemory;

public class InMemoryValueObjectRepositoryStrategy<TItem>
    : ValueObjectRepositoryStrategy<TItem> {

    #region Blocking

    public override void Seed(IEnumerable<TItem> seed)
        => Data = seed as List<TItem> ?? IsNotNull(seed).ToList();
    public override void Load() { }

    public override TItem[] GetAll()
        => Query.ToArray();

    public override Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize) {
        var count = Query.Count();
        var items = Query.Skip((int)(pageIndex * pageSize))
                   .Take((int)pageSize)
                   .ToArray();
        return new() {
            TotalCount = (uint)count,
            Index = pageIndex,
            Size = pageSize,
            Items = items,
        };
    }

    public override Block<TItem> GetBlock(Expression<Func<TItem, bool>> isNotStart, uint blockSize = DefaultBlockSize) {
        var items = Query.SkipWhile(isNotStart)
                   .Take((int)blockSize)
                   .ToArray();
        return new() {
            Size = blockSize,
            Items = items,
        };
    }

    public override TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Query.FirstOrDefault(predicate);

    public override TItem Create(Action<TItem> setItem) {
        var item = Activator.CreateInstance<TItem>();
        setItem(item);
        return item;
    }

    public override void Add(TItem newItem)
        => Data.Add(newItem);

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) {
        if (!TryRemove(predicate))
            return;
        Add(updatedItem);
    }

    public override void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem) {
        var itemToPatch = Query.FirstOrDefault(predicate);
        if (itemToPatch is null)
            return;
        setItem(itemToPatch);
    }

    public override void Remove(Expression<Func<TItem, bool>> predicate)
        => TryRemove(predicate);

    private bool TryRemove(Expression<Func<TItem, bool>> predicate) {
        var itemToRemove = Query.FirstOrDefault(predicate);
        if (itemToRemove is null)
            return false;
        Data.Remove(itemToRemove);
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
        => AsyncQuery.ToArrayAsync(ct);

    public override async ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, CancellationToken ct = default) {
        var count = await AsyncQuery.CountAsync(ct);
        var items = await AsyncQuery.Skip((int)(pageIndex * pageSize))
                   .Take((int)pageSize)
                   .ToArrayAsync(ct);
        return new() {
            TotalCount = (uint)count,
            Index = pageIndex,
            Size = pageSize,
            Items = items,
        };
    }

    public override async ValueTask<Block<TItem>> GetBlockAsync(Expression<Func<TItem, bool>> isNotStart, uint blockSize = DefaultBlockSize, CancellationToken ct = default) {
        var items = await AsyncQuery.SkipWhile(isNotStart)
                   .Take((int)blockSize)
                   .ToArrayAsync(ct);
        return new() {
            Size = blockSize,
            Items = items,
        };
    }

    public override ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Query.FirstOrDefaultAsync(predicate, ct);

    public override async Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) {
        var item = Activator.CreateInstance<TItem>();
        await setItem(item, ct);
        return item;
    }

    public override Task AddAsync(TItem newItem, CancellationToken ct = default) {
        Data.Add(newItem);
        return Task.CompletedTask;
    }

    public override async Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) {
        if (!await TryRemoveAsync(predicate, ct))
            return;
        await AddAsync(updatedItem, ct);
    }

    public override async Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) {
        var itemToPatch = await AsyncQuery.FirstOrDefaultAsync(predicate, ct);
        if (itemToPatch is null)
            return;
        await setItem(itemToPatch, ct);
    }

    public override Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => TryRemoveAsync(predicate, ct);

    private async Task<bool> TryRemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        var itemToRemove = await AsyncQuery.FirstOrDefaultAsync(predicate, ct);
        if (itemToRemove is null)
            return false;
        Data.Remove(itemToRemove);
        return true;
    }

    #endregion
}
