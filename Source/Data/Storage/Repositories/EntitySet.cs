namespace DotNetToolbox.Data.Repositories;

public class EntitySet<TModel>
    : IEntitySet<TModel> {
    private readonly Expression _expression;
    private readonly IEntitySetQueryHandler _provider;
    public EntitySet(Expression? expression, IEnumerable<TModel>? source, IRepositoryStrategy? strategy) {
        Strategy = strategy ?? new InMemoryRepositoryStrategy();
        _provider = new EntitySetQueryHandler(Strategy);
        Local = new(source ?? []);
        _expression = expression ?? Expression.Constant(Local);
    }

    protected IRepositoryStrategy Strategy { get; }
    internal EnumerableQuery<TModel> Local { get; }
    public Type ElementType { get; } = typeof(TModel);
    public IAsyncEnumerator<TModel> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => _provider
            .ExecuteAsync<IAsyncEnumerable<TModel>>(_expression, cancellationToken)
            .GetAsyncEnumerator(cancellationToken);
    public IEnumerator<TModel> GetEnumerator()
        => _provider.Execute<IEnumerable<TModel>>(_expression).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public virtual Expression Expression => _expression;
    IQueryProvider IQueryable.Provider => Provider;
    public IEntitySetQueryHandler Provider => _provider;
}
