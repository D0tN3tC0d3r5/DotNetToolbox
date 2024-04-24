namespace DotNetToolbox.Linq.Async;

public interface IAsyncQueryProvider {
    IQueryable CreateQuery(Expression expression);
    IQueryable<TElement> CreateQuery<TElement>(Expression expression);
    Task<object?> ExecuteAsync(Expression expression);
    Task<TResult> ExecuteAsync<TResult>(Expression expression);
}
