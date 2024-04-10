namespace DotNetToolbox.Data.Repositories;

public interface IRepositoryStrategy {
    IEntitySet Create(Expression expression);
    IEntitySet<TResult> Create<TResult>(Expression expression);
    TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken);
    Task<TResult> ExecuteAsync<TResult>(string command, CancellationToken cancellationToken);
    Task<TResult> ExecuteAsync<TInput, TResult>(string command, TInput input, CancellationToken cancellationToken);
    Task ExecuteAsync<TInput>(string command, TInput input, CancellationToken cancellationToken);

    Task<TResult> ExecuteAsync<TResult>(string command, Expression expression, CancellationToken cancellationToken);
    Task<TResult> ExecuteAsync<TInput, TResult>(string command, TInput input, Expression expression, CancellationToken cancellationToken);
    Task ExecuteAsync<TInput>(string command, TInput input, Expression expression, CancellationToken cancellationToken);
}
