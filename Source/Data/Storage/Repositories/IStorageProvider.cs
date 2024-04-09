namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlyStorageProvider
    : IAsyncQueryProvider {
    IQueryableSet Create(Expression expression);
    IQueryableSet<TElement> Create<TElement>(Expression expression);
    TResult ExecuteQuery<TResult>(Expression expression);
    TResult ExecuteQueryAsync<TResult>(Expression expression, CancellationToken cancellationToken = default);
}
public interface IStorageProvider
    : IReadOnlyStorageProvider {
    TResult ExecuteCommand<TResult>();
    TResult ExecuteCommandAsync<TResult>(CancellationToken cancellationToken = default);
}
