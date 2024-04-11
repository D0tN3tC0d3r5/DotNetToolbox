namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategy<TEntity>
    : IRepositoryStrategy {
    protected IEnumerable<TEntity> Source { get; }

    public InMemoryRepositoryStrategy(IEnumerable<TEntity>? source = null) {
        Source = source ?? [];
    }

    public IItemSet Create(Expression expression)
        => ItemSet.Create(expression.Type, expression, this);
    public IItemSet<TResult> Create<TResult>(Expression expression)
        => new ItemSet<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public Task<TResult> ExecuteAsync<TInput, TResult>(string command, TInput input, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    public Task<TResult> ExecuteAsync<TInput, TResult>(string command, TInput input, Expression expression, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public Task<TResult> ExecuteAsync<TResult>(string command, CancellationToken cancellationToken)
        => ExecuteAsync<TResult>(command, default(Expression)!, cancellationToken);

    public async Task<TResult> ExecuteAsync<TResult>(string command, Expression expression, CancellationToken cancellationToken)
        => (TResult?)await ExecuteStrategyAsync(command, expression, default, cancellationToken);

    public Task ExecuteAsync<TInput>(string command, TInput input, Expression expression, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    public async Task ExecuteAsync<TInput>(string command, TInput input, CancellationToken cancellationToken)
        => await ExecuteStrategyAsync(command, default, input, cancellationToken);

    private async Task<object?> ExecuteStrategyAsync(string command, Expression? expression, object? input, CancellationToken ct)
        => IsNotNull(command) switch {
            "Count" when Source is ICollection c => c.Count,
            "Count" => Source.Count(),
            "Any" => Source.Any(),
            "FindFirst" => Source.First(),
            "GetList" => Source.ToArray(),
            _ => throw new NotSupportedException($"Command '{command}' is not supported."),
        };
}
