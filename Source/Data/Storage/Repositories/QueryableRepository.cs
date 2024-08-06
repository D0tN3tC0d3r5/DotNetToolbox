namespace DotNetToolbox.Data.Repositories;

public abstract class QueryableRepository<TItem>
    : IQueryableRepository<TItem> {
    internal string Id { get; } = $"|>Repository[{typeof(TItem).Name}]_{Guid.NewGuid():N}<|";

    internal List<TItem> Data { get; set; } = [];
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
