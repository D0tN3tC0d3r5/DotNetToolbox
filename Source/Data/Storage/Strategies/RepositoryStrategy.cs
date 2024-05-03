namespace DotNetToolbox.Data.Strategies;

public abstract class RepositoryStrategy<TItem>
    : IRepositoryStrategy {

    protected List<TItem> Data { get; set; } = [];
    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;

    #region Blocking

    protected IQueryable<TItem> Query => Data.AsQueryable();
    public IQueryProvider Provider => Query.Provider;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => Query.GetEnumerator();

    public virtual void Seed(IEnumerable<TItem> seed)
        => throw new NotImplementedException();
    public virtual void Load()
        => throw new NotImplementedException();

    #endregion

    #region Async

    protected IAsyncQueryable<TItem> AsyncQuery => Data.AsAsyncQueryable();
    public IAsyncQueryProvider AsyncProvider => AsyncQuery.AsyncProvider;
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => AsyncQuery.GetAsyncEnumerator(ct);

    public virtual Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task LoadAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

#endregion
}
