namespace System.Linq.Async;

public interface IAsyncQueryProvider {
    IAsyncQueryable CreateAsyncQuery(Expression expression);
    Task<object?> ExecuteAsync(Expression expression, CancellationToken ct = default);
    IAsyncQueryable<TElement> CreateAsyncQuery<TElement>(Expression expression);
    Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken ct = default);
}
