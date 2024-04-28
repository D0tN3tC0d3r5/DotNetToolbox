namespace DotNetToolbox.Data.Repositories;

public class AsyncRepository<TItem>
    : AsyncRepository<IAsyncRepositoryStrategy<TItem>, TItem> {
    public AsyncRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryAsyncRepositoryStrategy<TItem>(), data) { }
    public AsyncRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base((IAsyncRepositoryStrategy<TItem>)IsNotNull(provider).GetStrategyFor<TItem>(), data) { }

    public AsyncRepository(IAsyncRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(IsNotNull(strategy), data ?? []) {
    }
}

public abstract class AsyncRepository<TStrategy, TItem>(TStrategy strategy, IEnumerable<TItem>? data = null) : Repository<TStrategy, TItem>(strategy, data), IAsyncRepository<TItem>
    where TStrategy : class, IAsyncRepositoryStrategy<TItem> {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1725:Parameter names should match base declaration", Justification = "<Pending>")]
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => Strategy.GetAsyncEnumerator(ct);

    public IAsyncQueryProvider AsyncProvider => Strategy.AsyncProvider;

    public Task SeedAsync(IAsyncEnumerable<TItem> seed, CancellationToken ct = default)
        => Strategy.SeedAsync(seed, ct);

    public Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Strategy.AddAsync(newItem, ct);
    public Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(predicate, updatedItem, ct);
    public Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.RemoveAsync(predicate, ct);
}
