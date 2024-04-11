namespace DotNetToolbox.Data.Repositories;

public interface IAsyncRepositoryStrategy<out TStrategy>
    : IQueryStrategy<TStrategy>
    where TStrategy : IAsyncRepositoryStrategy<TStrategy> {
    //IAsyncItemSet<TResult> Create<TResult>(Expression expression);
    Task<TResult> ExecuteFunction<TResult>(string command, object? input, CancellationToken ct);
    Task ExecuteActionAsync(string command, object? input, CancellationToken ct = default);
}
