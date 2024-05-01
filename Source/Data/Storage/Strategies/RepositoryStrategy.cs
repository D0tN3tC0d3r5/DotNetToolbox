namespace DotNetToolbox.Data.Strategies;

public abstract class RepositoryStrategy<TItem>
    : IRepositoryStrategy {
    protected List<TItem> OriginalData { get; set; } = [];
    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;

    #region Blocking

    protected IQueryable<TItem> Query => OriginalData.AsQueryable();
    public IQueryProvider Provider => Query.Provider;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => Query.GetEnumerator();

    public virtual void Seed(IEnumerable<TItem> seed)
        => OriginalData = seed as List<TItem> ?? IsNotNull(seed).ToList();

    #endregion

    #region Async

    protected IAsyncQueryable<TItem> AsyncQuery => OriginalData.AsAsyncQueryable();
    public IAsyncQueryProvider AsyncProvider => AsyncQuery.AsyncProvider;
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => AsyncQuery.GetAsyncEnumerator(ct);

    public virtual Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default) {
        Seed(seed);
        return Task.CompletedTask;
    }

    #endregion
}
