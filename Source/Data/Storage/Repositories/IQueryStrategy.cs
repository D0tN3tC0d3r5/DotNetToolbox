namespace DotNetToolbox.Data.Repositories;

public interface IQueryStrategy {
    IItemSet Create(LambdaExpression expression);
}

public interface IQueryStrategy<out TStrategy>
    : IQueryStrategy
    where TStrategy : IQueryStrategy<TStrategy> {
    TResult ExecuteQuery<TResult>(LambdaExpression expression);
}
