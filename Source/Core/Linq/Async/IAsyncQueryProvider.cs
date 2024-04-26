// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public interface IAsyncQueryProvider
    : IQueryProvider {
    IAsyncQueryable<TElement> CreateAsyncQuery<TElement>(Expression expression);
    Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken ct = default);
}
