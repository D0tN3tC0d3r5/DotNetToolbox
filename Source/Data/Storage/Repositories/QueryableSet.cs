namespace DotNetToolbox.Data.Repositories;

public class QueryableSet<TModel>
    : IQueryableSet<TModel> {
    private readonly Expression _expression;
    private readonly ReadOnlyStorageProvider _provider;
    public QueryableSet(IQueryStrategy? strategy, IEnumerable<TModel>? source, Expression? expression) {
        _provider = new(strategy ?? new InMemoryStorageStrategy());
        Local = new(source ?? []);
        _expression = expression ?? Expression.Constant(Local);
    }

    internal EnumerableQuery<TModel> Local { get; }
    public Type ElementType { get; } = typeof(TModel);
    public IAsyncEnumerator<TModel> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => _provider
            .ExecuteQueryAsync<IAsyncEnumerable<TModel>>(_expression, cancellationToken)
            .GetAsyncEnumerator(cancellationToken);
    public IEnumerator<TModel> GetEnumerator()
        => _provider.ExecuteQuery<IEnumerable<TModel>>(_expression).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public virtual Expression Expression => _expression;
    IQueryProvider IQueryable.Provider => Provider;
    public IReadOnlyStorageProvider Provider => _provider;
}
