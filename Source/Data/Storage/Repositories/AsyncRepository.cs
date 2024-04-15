namespace DotNetToolbox.Data.Repositories;

public class AsyncRepository<TItem>
    : IAsyncRepository<TItem>
    where TItem : class {
    private readonly IQueryable<TItem> _data;

    public AsyncRepository(IStrategyProvider? provider = null)
        : this([], provider) {
    }

    public AsyncRepository(Expression expression, IStrategyProvider? provider = null) {
        _data = new EnumerableQuery<TItem>(expression);
    }

    public AsyncRepository(IEnumerable<TItem> source, IStrategyProvider? provider = null) {
        // ReSharper disable PossibleMultipleEnumeration
        _data = IsNotNull(source).AsQueryable();
        // ReSharper enable PossibleMultipleEnumeration
    }

    public Type ElementType => _data.ElementType;
    public Expression Expression => _data.Expression;

    IQueryProvider IQueryable.Provider => Provider;
    public IAsyncRepositoryStrategy<TItem> Provider => throw new NotImplementedException();

    public IEnumerator<TItem> GetEnumerator() => _data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => ((IAsyncEnumerable<TItem>)_data).GetAsyncEnumerator(ct);

    public Task<bool> HaveAny(CancellationToken ct)
        => Provider.HaveAny(ct);
    public Task<int> Count(CancellationToken ct)
        => Provider.Count(ct);
    public Task<TItem[]> ToArray(CancellationToken ct)
        => Provider.ToArray(ct);
    public Task<TItem?> GetFirst(CancellationToken ct)
        => Provider.GetFirst(ct);

    public Task Add(TItem newItem, CancellationToken ct = default)
        => Provider.Add(newItem, ct);
    public Task Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => Provider.Update(predicate, updatedItem, ct);
    public Task Remove(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Provider.Remove(predicate, ct);
}
