namespace DotNetToolbox.Data.Repositories;

public interface IQueryStrategy<out TStrategy>
    where TStrategy : IQueryStrategy<TStrategy> {
    TResult ExecuteQuery<TResult>(Expression expression, CancellationToken ct);
    //IItemSet Create(Expression expression);
}
