namespace DotNetToolbox.Data.Repositories;

public interface IEntitySetQueryHandler
    : IAsyncQueryProvider {
    IEntitySet Create(Expression expression);
    IEntitySet<TElement> Create<TElement>(Expression expression);
    new TResult Execute<TResult>(Expression expression);
    TResult Execute<TResult>(string command, Expression expression);
    new TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default);
    Task<TResult> ExecuteAsync<TResult>(string command, Expression expression, CancellationToken cancellationToken = default);
}

public interface IEntitySetCommandHandler {
    TResult Execute<TResult>(string command);
    Task<TResult> ExecuteAsync<TResult>(string command, CancellationToken cancellationToken = default);
}
