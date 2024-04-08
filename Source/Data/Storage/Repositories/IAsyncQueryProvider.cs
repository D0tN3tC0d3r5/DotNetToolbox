namespace DotNetToolbox.Data.Repositories;

public interface IAsyncQueryProvider
    : IQueryProvider {
    TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default);
}
