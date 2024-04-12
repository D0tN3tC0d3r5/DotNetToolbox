namespace DotNetToolbox.Data.Repositories;

public interface IQueryStrategy {
    IItemSet Create(Expression expression);
}

public interface IQueryStrategy<out TStrategy>
    : IQueryStrategy
    where TStrategy : IQueryStrategy<TStrategy> {
    TResult ExecuteQuery<TResult>(Expression expression);
}
