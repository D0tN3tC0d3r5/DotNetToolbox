namespace DotNetToolbox.Data.DataSources;

public abstract class QueryableDataSource<TItem>(IEnumerable<TItem>? records = null)
    : IQueryableDataSource<TItem> {
    internal string Id { get; } = $"|>Data[{typeof(TItem).Name}]_{GuidProvider.Default.CreateSortable():N}<|";

    public List<TItem> Records { get; } = records?.ToList() ?? [];
    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;

    #region Blocking

    internal IQueryable<TItem> Query => Records.AsQueryable();
    public IQueryProvider Provider => Query.Provider;
    IEnumerator IEnumerable.GetEnumerator()
        => Query.GetEnumerator();
    public IEnumerator<TItem> GetEnumerator()
        => Query.GetEnumerator();

    #endregion

    #region Async

    internal IAsyncQueryable<TItem> AsyncQuery => Records.AsAsyncQueryable();
    public IAsyncQueryProvider AsyncProvider => AsyncQuery.AsyncProvider;
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => AsyncQuery.GetAsyncEnumerator(cancellationToken);

    #endregion
}
