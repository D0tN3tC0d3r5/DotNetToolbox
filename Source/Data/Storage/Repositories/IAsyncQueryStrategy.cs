namespace DotNetToolbox.Data.Repositories;

public interface IAsyncQueryStrategy<out TStrategy>
    : IQueryStrategy
    where TStrategy : IAsyncQueryStrategy<TStrategy> {
    TResult ExecuteQuery<TResult>(LambdaExpression expression, CancellationToken ct);
}
