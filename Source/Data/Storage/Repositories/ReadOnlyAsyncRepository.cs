namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyAsyncRepository<TItem>(IEnumerable<TItem>? source = null, IAsyncRepositoryStrategy<TItem>? strategy = null)
    : IReadOnlyAsyncRepository<TItem>
    where TItem : class {
    private readonly IQueryable<TItem> _data = new List<TItem>(source ?? []).AsQueryable();

    public Type ElementType => _data.ElementType;
    public Expression Expression => _data.Expression;
    public IQueryProvider Provider => _data.Provider;

    protected IAsyncRepositoryStrategy<TItem> Strategy { get; } = IsNotNull(strategy);

    public IEnumerator<TItem> GetEnumerator() => _data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => ((IAsyncEnumerable<TItem>)_data).GetAsyncEnumerator(ct);
    public Task<bool> HaveAny(CancellationToken ct)
        => Strategy.HaveAny(ct);
    public Task<int> Count(CancellationToken ct)
        => Strategy.Count(ct);
    public Task<TItem[]> ToArray(CancellationToken ct)
        => Strategy.ToArray(ct);
    public Task<TItem?> GetFirst(CancellationToken ct)
        => Strategy.GetFirst(ct);
}
