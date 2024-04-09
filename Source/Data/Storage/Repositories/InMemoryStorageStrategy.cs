namespace DotNetToolbox.Data.Repositories;

public class InMemoryStorageStrategy
    : IQueryStrategy {
    public TResult ExecuteQuery<TResult>(Expression query) => throw new NotImplementedException();

    public TResult ExecuteQueryAsync<TResult>(Expression query, CancellationToken cancellationToken) => throw new NotImplementedException();
}
