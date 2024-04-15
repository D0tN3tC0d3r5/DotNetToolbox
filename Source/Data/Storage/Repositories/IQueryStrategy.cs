namespace DotNetToolbox.Data.Repositories;

public interface IQueryStrategy
    : IQueryProvider {
    IRepository CreateRepository(Expression expression);
}

public interface IQueryStrategy<TItem>
    : IQueryStrategy
    where TItem : class {
    new IRepository<TItem> CreateRepository(Expression expression);
    IRepository<TResult> CreateRepository<TResult>(Expression expression)
        where TResult : class;
}
