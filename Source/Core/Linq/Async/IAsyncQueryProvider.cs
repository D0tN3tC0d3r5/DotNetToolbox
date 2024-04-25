namespace DotNetToolbox.Linq.Async;

public interface IAsyncQueryProvider
    : IQueryProvider {
    //IAsyncQueryable CreateAsyncQuery(Expression expression);
    IAsyncQueryable<TElement> CreateAsyncQuery<TElement>(Expression expression);
    //Task<object?> ExecuteAsync(Expression expression, CancellationToken ct = default);
    Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken ct = default);
}
