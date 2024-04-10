namespace DotNetToolbox.Data.Repositories;

public class EntitySetHandler(IRepositoryStrategy strategy)
        : EntitySetQueryHandler(strategy),
          IEntitySetCommandHandler {
    public TResult Execute<TResult>(string command)
        => Strategy.Execute<TResult>(command);
    public Task<TResult> ExecuteAsync<TResult>(string command, CancellationToken cancellationToken = default)
        => Strategy.ExecuteAsync<TResult>(command, cancellationToken);
}
