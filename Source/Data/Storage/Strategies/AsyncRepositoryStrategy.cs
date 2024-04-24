namespace DotNetToolbox.Data.Strategies;

public abstract class AsyncRepositoryStrategy
    : IAsyncRepositoryStrategy;

public abstract class AsyncRepositoryStrategy<TItem>
    : AsyncRepositoryStrategy,
    IAsyncRepositoryStrategy<TItem> {

    protected AsyncRepositoryStrategy()
        : this([]) {
    }

    protected AsyncRepositoryStrategy(IEnumerable<TItem> data) {
        OriginalData = data.ToList();
        Query = OriginalData.AsAsyncQueryable();
    }

    public System.Collections.Async.Generic.IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default) => Query.GetEnumerator();
    IAsyncEnumerator IAsyncEnumerable.GetAsyncEnumerator(CancellationToken ct) => GetAsyncEnumerator(ct);

    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;
    public IAsyncQueryProvider Provider => Query.Provider;

    protected IAsyncQueryable<TItem> Query { get; set; }
    protected IEnumerable<TItem> OriginalData { get; set; }
    protected List<TItem> UpdatableData => IsOfType<List<TItem>>(OriginalData);

    public virtual Task SeedAsync(IEnumerable<TItem> seed)
        => throw new NotImplementedException();

    public virtual Task AddAsync(TItem newItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
}
