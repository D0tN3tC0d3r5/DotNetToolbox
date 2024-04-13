namespace DotNetToolbox.Data.Repositories;

public interface IAsyncRepositoryStrategy<out TStrategy>
    : IAsyncQueryStrategy<TStrategy>
    where TStrategy : IAsyncRepositoryStrategy<TStrategy> {
    Task<TResult> ExecuteFunction<TResult>(string command, object? input, CancellationToken ct);
    Task ExecuteActionAsync(string command, object? input, CancellationToken ct = default);
}
