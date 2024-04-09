namespace DotNetToolbox.Data.Repositories;

public interface IUpdateStrategy {
    TResult ExecuteCommand<TResult>();
    TResult ExecuteCommandAsync<TResult>(CancellationToken cancellationToken);
}

public interface IQueryStrategy {
    TResult ExecuteQuery<TResult>(Expression query);
    TResult ExecuteQueryAsync<TResult>(Expression query, CancellationToken cancellationToken);
}
