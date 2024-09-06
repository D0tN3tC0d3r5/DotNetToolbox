namespace DotNetToolbox.Data.Repositories;

public abstract class QueryableRepository<TItem>(IEnumerable<TItem>? data = null)
    : IQueryableRepository<TItem> {
    internal string Id { get; } = $"|>Repository[{typeof(TItem).Name}]_{GuidProvider.Default.CreateSortable():N}<|";

    public List<TItem> Data { get; internal set; } = data?.ToList() ?? [];
    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;

    #region Blocking

    internal IQueryable<TItem> Query => Data.AsQueryable();
    public IQueryProvider Provider => Query.Provider;
    IEnumerator IEnumerable.GetEnumerator()
        => Query.GetEnumerator();
    public IEnumerator<TItem> GetEnumerator()
        => Query.GetEnumerator();

    #endregion

    #region Async

    internal IAsyncQueryable<TItem> AsyncQuery => Data.AsAsyncQueryable();
    public IAsyncQueryProvider AsyncProvider => AsyncQuery.AsyncProvider;
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => AsyncQuery.GetAsyncEnumerator(cancellationToken);

    #endregion
}
