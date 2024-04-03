// ReSharper disable once CheckNamespace - This is intended
namespace System.Linq.Async;

public interface IAsyncQueryProvider : IQueryProvider {
    TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default);
}
