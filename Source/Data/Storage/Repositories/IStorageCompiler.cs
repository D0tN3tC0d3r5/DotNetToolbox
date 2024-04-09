namespace DotNetToolbox.Data.Repositories;

public interface IStorageCompiler {
    TResult Execute<TResult>(Expression query);
    TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken);
}
