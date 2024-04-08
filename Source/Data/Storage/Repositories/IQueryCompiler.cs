namespace DotNetToolbox.Data.Repositories;

public interface IQueryCompiler {
    TResult Execute<TResult>(Expression query);
    TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken);
}
