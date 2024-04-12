namespace DotNetToolbox.Data.Repositories;

public interface IAsyncQueryStrategy<out TStrategy>
    : IQueryStrategy
    where TStrategy : IAsyncQueryStrategy<TStrategy> {
    TResult ExecuteQuery<TResult>(Expression expression, CancellationToken ct);
}
