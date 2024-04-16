namespace DotNetToolbox.Data.Repositories;

public interface IQueryStrategy
    : IQueryProvider {
    IRepository CreateRepository(Expression expression);
    IRepository<TResult> CreateRepository<TResult>(Expression expression)
        where TResult : class;
}
