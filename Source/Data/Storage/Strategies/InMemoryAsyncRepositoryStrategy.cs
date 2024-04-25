namespace DotNetToolbox.Data.Strategies;

public class InMemoryAsyncRepositoryStrategy<TItem>
    : AsyncRepositoryStrategy<TItem> {

    public InMemoryAsyncRepositoryStrategy() { }
    public InMemoryAsyncRepositoryStrategy(IEnumerable<TItem> data)
        : base(data) { }

    public override void Seed(IEnumerable<TItem> seed) {
        OriginalData = seed.ToList();
        Query = OriginalData.AsQueryable();
        AsyncQuery = OriginalData.AsAsyncQueryable();
    }

    public override async Task SeedAsync(IAsyncEnumerable<TItem> seed, CancellationToken ct = default) {
        OriginalData = await seed.ToListAsync(ct);
        Query = OriginalData.AsQueryable();
        AsyncQuery = OriginalData.AsAsyncQueryable();
    }

    public override void Add(TItem newItem) {
        UpdatableData.Add(newItem);
        Query = OriginalData.AsQueryable();
        AsyncQuery = OriginalData.AsAsyncQueryable();
    }

    public override Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Task.Run(() => Add(newItem), ct);

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) {
        Remove(predicate);
        Add(updatedItem);
    }

    public override async Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) {
        await RemoveAsync(predicate, ct);
        await AddAsync(updatedItem, ct);
    }

    public override void Remove(Expression<Func<TItem, bool>> predicate) {
        var itemToRemove = Query.FirstOrDefault(predicate);
        if (itemToRemove is null) return;
        UpdatableData.Remove(itemToRemove);
        Query = OriginalData.AsQueryable();
        AsyncQuery = OriginalData.AsAsyncQueryable();
    }

    public override async Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        var itemToRemove = await AsyncQuery.FirstOrDefaultAsync(predicate.Compile(), ct);
        if (itemToRemove is null) return;
        UpdatableData.Remove(itemToRemove);
        Query = OriginalData.AsQueryable();
        AsyncQuery = OriginalData.AsAsyncQueryable();
    }
}
