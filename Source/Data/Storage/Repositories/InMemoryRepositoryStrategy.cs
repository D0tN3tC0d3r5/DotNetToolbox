namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategy
    : IRepositoryStrategy {
    public TResult Execute<TResult>(Expression query)
        => throw new NotImplementedException();
    public TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public TResult Execute<TResult>(string command)
        => throw new NotImplementedException();
    public Task<TResult> ExecuteAsync<TResult>(string command, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public TResult Execute<TInput, TResult>(string command, TInput input)
        => throw new NotImplementedException();
    public Task<TResult> ExecuteAsync<TInput, TResult>(string command, TInput input, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public void Execute<TInput>(string command, TInput input)
        => throw new NotImplementedException();
    public Task ExecuteAsync<TInput>(string command, TInput input, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public TResult Execute<TResult>(string command, Expression query)
        => throw new NotImplementedException();
    public Task<TResult> ExecuteAsync<TResult>(string command, Expression query, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public TResult Execute<TInput, TResult>(string command, TInput input, Expression query)
        => throw new NotImplementedException();
    public Task<TResult> ExecuteAsync<TInput, TResult>(string command, TInput input, Expression query, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public void Execute<TInput>(string command, TInput input, Expression query)
        => throw new NotImplementedException();
    public Task ExecuteAsync<TInput>(string command, TInput input, Expression query, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
