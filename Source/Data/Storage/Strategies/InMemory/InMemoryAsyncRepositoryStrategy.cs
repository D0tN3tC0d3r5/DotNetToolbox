namespace DotNetToolbox.Data.Strategies.InMemory;

public class InMemoryAsyncRepositoryStrategy<TItem>
    : InMemoryRepositoryStrategy<TItem> {
    public InMemoryAsyncRepositoryStrategy() {
    }

    public InMemoryAsyncRepositoryStrategy(IEnumerable<TItem> data)
        : base(data) {
    }

    public override void Seed(IEnumerable<TItem> seed) {
        base.Seed(seed);
        AsyncQuery = OriginalData.ToAsyncQueryable();
    }

    public override async Task SeedAsync(IAsyncEnumerable<TItem> seed, CancellationToken ct = default) {
        var list = new List<TItem>();
        await foreach (var item in seed.WithCancellation(ct))
            list.Add(item);
        OriginalData = list;
        Query = OriginalData.AsQueryable();
        AsyncQuery = OriginalData.ToAsyncQueryable();
    }

    public override void Add(TItem newItem) {
        base.Add(newItem);
        AsyncQuery = OriginalData.ToAsyncQueryable();
    }

    public override Task AddAsync(TItem newItem, CancellationToken ct = default) {
        Add(newItem);
        AsyncQuery = OriginalData.ToAsyncQueryable();
        return Task.CompletedTask;
    }

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) {
        base.Update(predicate, updatedItem);
        AsyncQuery = OriginalData.ToAsyncQueryable();
    }

    public override async Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) {
        if (!await TryRemoveAsync(predicate, ct))
            return;
        await AddAsync(updatedItem, ct);
        AsyncQuery = OriginalData.ToAsyncQueryable();
    }

    public override void Remove(Expression<Func<TItem, bool>> predicate) {
        base.Remove(predicate);
        AsyncQuery = OriginalData.ToAsyncQueryable();
    }

    public override async Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        if (!await TryRemoveAsync(predicate, ct))
            return;
        AsyncQuery = OriginalData.ToAsyncQueryable();
    }

    private async Task<bool> TryRemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        var itemToRemove = await AsyncQuery.FirstOrDefaultAsync(predicate.Compile(), ct);
        if (itemToRemove is null)
            return false;
        UpdatableData.Remove(itemToRemove);
        Query = OriginalData.AsQueryable();
        return true;
    }
}
