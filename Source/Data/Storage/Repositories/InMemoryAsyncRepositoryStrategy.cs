namespace DotNetToolbox.Data.Repositories;

public class InMemoryAsyncRepositoryStrategy<TItem>(IAsyncItemSet<TItem, InMemoryAsyncRepositoryStrategy<TItem>> repository)
    : IAsyncRepositoryStrategy<InMemoryAsyncRepositoryStrategy<TItem>> {
    protected AsyncItemSet<TItem> Repository { get; } = (AsyncItemSet<TItem>)repository;

    public IItemSet Create(Expression expression)
        => AsyncItemSet.Create(expression.Type, expression, this);
    public IAsyncItemSet<TResult> Create<TResult>(Expression expression)
        => new AsyncItemSet<TResult>(expression);

    public TResult ExecuteQuery<TResult>(Expression expression, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public Task<TResult> ExecuteFunction<TResult>(string command, object? input, CancellationToken ct = default)
        => throw new NotSupportedException($"Command '{command}' is not supported for a '{Repository.GetType().Name}<{typeof(TItem).Name}>'.");
    public Task ExecuteActionAsync(string command, object? input, CancellationToken ct = default)
        => throw new NotSupportedException($"Command '{command}' is not supported for a '{Repository.GetType().Name}<{typeof(TItem).Name}>'.");
}
