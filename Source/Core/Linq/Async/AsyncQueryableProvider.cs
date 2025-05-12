namespace System.Linq.Async;

public class AsyncQueryableProvider(IEnumerable source) : IAsyncQueryProvider {
    private readonly IQueryable _source = source.AsQueryable();
    IAsyncQueryable IAsyncQueryProvider.CreateAsyncQuery(Expression expression) => new AsyncQueryable(Enumerable.Empty<object>(), expression);
    Task<object?> IAsyncQueryProvider.ExecuteAsync(Expression expression, CancellationToken ct)
        => Task.Run(() => _source.Provider.Execute(expression), ct);
    IAsyncQueryable<TElement> IAsyncQueryProvider.CreateAsyncQuery<TElement>(Expression expression)
        => new AsyncQueryable<TElement>([], expression);
    Task<TResult> IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken ct)
        => Task.Run(() => _source.Provider.Execute<TResult>(expression), ct);
}
