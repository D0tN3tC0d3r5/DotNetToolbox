namespace DotNetToolbox.Data.Repositories;
public class AsyncRepository<TStrategy, TItem>
    : AsyncRepository<AsyncRepository<TStrategy, TItem>, TStrategy, TItem>
    where TStrategy : class, IAsyncRepositoryStrategy<TItem>{
    public AsyncRepository(TStrategy strategy)
        : base(strategy) { }
    public AsyncRepository(IStrategyFactory factory)
        : base(factory) { }
    public AsyncRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : base(data, factory) { }
    public AsyncRepository(IEnumerable<TItem> data, TStrategy strategy)
        : base(data, strategy) {
    }
}

public class AsyncRepository<TRepository, TStrategy, TItem>
    : IAsyncRepository<TItem>
    where TRepository : AsyncRepository<TRepository, TStrategy, TItem>
    where TStrategy : class, IAsyncRepositoryStrategy<TItem>{

    public AsyncRepository(TStrategy strategy)
        : this([], strategy) {
    }
    public AsyncRepository(IStrategyFactory factory)
        : this([], factory) {
    }
    public AsyncRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(data, factory.GetRequiredAsyncStrategy<TItem, TStrategy>()) {
    }
    // ReSharper disable PossibleMultipleEnumeration
    public AsyncRepository(IEnumerable<TItem> data, TStrategy strategy) {
        Strategy = IsNotNull(strategy);
        Strategy.SeedAsync(data.ToList()).GetAwaiter().GetResult();
    }
    // ReSharper enable PossibleMultipleEnumeration

    protected TStrategy Strategy { get; }

    IAsyncEnumerator IAsyncEnumerable.GetAsyncEnumerator(CancellationToken ct) => GetAsyncEnumerator(ct);
    public System.Collections.Async.Generic.IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => Strategy.GetAsyncEnumerator(ct);

    public Type ElementType => Strategy.ElementType;
    public Expression Expression => Strategy.Expression;
    public IAsyncQueryProvider Provider => Strategy.Provider;

    public Task SeedAsync(IEnumerable<TItem> seed)
        => Strategy.SeedAsync(seed);

    public Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Strategy.AddAsync(newItem, ct);
    public Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(predicate, updatedItem, ct);
    public Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.RemoveAsync(predicate, ct);
}
