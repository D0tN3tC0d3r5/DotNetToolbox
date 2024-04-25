namespace DotNetToolbox.Data.Repositories;

public abstract class AsyncRepository<TStrategy, TItem>
    : AsyncRepository<AsyncRepository<TStrategy, TItem>, TStrategy, TItem>
    where TStrategy : class, IAsyncRepositoryStrategy<TItem>{
    protected AsyncRepository(TStrategy strategy)
        : base(strategy) { }
    protected AsyncRepository(IStrategyFactory factory)
        : base(factory) { }
    protected AsyncRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : base(data, factory) { }
    protected AsyncRepository(IEnumerable<TItem> data, TStrategy strategy)
        : base(data, strategy) {
    }
}

public abstract class AsyncRepository<TRepository, TStrategy, TItem>(IEnumerable<TItem> data, TStrategy strategy)
    : Repository<TRepository, TStrategy, TItem>(data, strategy)
    , IAsyncRepository<TItem>
    where TRepository : AsyncRepository<TRepository, TStrategy, TItem>
    where TStrategy : class, IAsyncRepositoryStrategy<TItem> {

    protected AsyncRepository(TStrategy strategy)
        : this([], strategy) {
    }
    protected AsyncRepository(IStrategyFactory factory)
        : this([], factory) {
    }
    protected AsyncRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(data, factory.GetRequiredAsyncStrategy<TItem, TStrategy>()) {
    }

    //IAsyncEnumerator IAsyncEnumerable.GetAsyncEnumerator(CancellationToken ct)
    //    => GetAsyncEnumerator(ct);
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
