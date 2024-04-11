namespace DotNetToolbox.Data.Repositories;

public interface IRepositoryStrategy<out TStrategy>
    : IQueryStrategy<TStrategy>
    where TStrategy : IRepositoryStrategy<TStrategy> {
    //IItemSet<TResult> Create<TResult>(Expression expression);
    TResult ExecuteFunction<TResult>(string command, object? input = null);
    void ExecuteAction(string command, object? input = null);
}
