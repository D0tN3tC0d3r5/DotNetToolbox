namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategy
    : IRepositoryStrategy {
    public IEntitySet Create(Expression expression)
        => EntitySet.Create(expression.Type,  expression, this);
    public IEntitySet<TResult> Create<TResult>(Expression expression) {
        var strategy = new InMemoryRepositoryStrategy();
        return new EntitySet<TResult>(expression, strategy);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    public Task<TResult> ExecuteAsync<TResult>(string command, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    public Task<TResult> ExecuteAsync<TInput, TResult>(string command, TInput input, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    public Task ExecuteAsync<TInput>(string command, TInput input, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    public Task<TResult> ExecuteAsync<TResult>(string command, Expression expression, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    public Task<TResult> ExecuteAsync<TInput, TResult>(string command, TInput input, Expression expression, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    public Task ExecuteAsync<TInput>(string command, TInput input, Expression expression, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
