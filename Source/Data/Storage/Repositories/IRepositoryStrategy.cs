namespace DotNetToolbox.Data.Repositories;

public interface IRepositoryStrategy {
    TResult Execute<TResult>(Expression query);
    TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken);

    TResult Execute<TResult>(string command);
    Task<TResult> ExecuteAsync<TResult>(string command, CancellationToken cancellationToken);
    TResult Execute<TInput, TResult>(string command, TInput input);
    Task<TResult> ExecuteAsync<TInput, TResult>(string command, TInput input, CancellationToken cancellationToken);
    void Execute<TInput>(string command, TInput input);
    Task ExecuteAsync<TInput>(string command, TInput input, CancellationToken cancellationToken);

    TResult Execute<TResult>(string command, Expression query);
    Task<TResult> ExecuteAsync<TResult>(string command, Expression query, CancellationToken cancellationToken);
    TResult Execute<TInput, TResult>(string command, TInput input, Expression query);
    Task<TResult> ExecuteAsync<TInput, TResult>(string command, TInput input, Expression query, CancellationToken cancellationToken);
    void Execute<TInput>(string command, TInput input, Expression query);
    Task ExecuteAsync<TInput>(string command, TInput input, Expression query, CancellationToken cancellationToken);
}
