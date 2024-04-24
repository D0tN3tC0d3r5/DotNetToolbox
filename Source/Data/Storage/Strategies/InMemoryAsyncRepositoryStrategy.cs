namespace DotNetToolbox.Data.Strategies;

public sealed class InMemoryAsyncRepositoryStrategy<TRepository, TItem>
    : AsyncRepositoryStrategy<TItem>
    where TRepository : IAsyncRepository<TItem>{

    public InMemoryAsyncRepositoryStrategy() {
    }

    public InMemoryAsyncRepositoryStrategy(IEnumerable<TItem> data)
        : base(data) {
    }

    public override Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default) {
        OriginalData = seed.ToList();
        Query = OriginalData.AsAsyncQueryable();
        return Task.CompletedTask;
    }

    public override Task AddAsync(TItem newItem, CancellationToken ct = default) {
        UpdatableData.Add(newItem);
        Query = OriginalData.AsAsyncQueryable();
        return Task.CompletedTask;
    }

    public override async Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) {
        await RemoveAsync(predicate, ct);
        await AddAsync(updatedItem, ct);
    }

    public override async Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        var itemToRemove = await Query.FirstOrDefaultAsync(predicate, ct);
        if (itemToRemove is null) return;
        UpdatableData.Add(itemToRemove);
        Query = OriginalData.AsAsyncQueryable();
    }
}
