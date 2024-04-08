namespace DotNetToolbox.Data.Repositories;

public class QueryableRepository<TModel>
    : IQueryableRepository<TModel> {
    private readonly ModelAsyncQueryProvider _queryProvider;
    public QueryableRepository(ModelAsyncQueryProvider queryProvider, Expression expression) {
        _queryProvider = queryProvider;
        Expression = expression;
    }
    public Type ElementType { get; } = typeof(TModel);
    public IAsyncEnumerator<TModel> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => _queryProvider
            .ExecuteAsync<IAsyncEnumerable<TModel>>(Expression, cancellationToken)
            .GetAsyncEnumerator(cancellationToken);
    public IEnumerator<TModel> GetEnumerator()
        => _queryProvider.Execute<IEnumerable<TModel>>(Expression).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public virtual Expression Expression { get; }
    public IQueryProvider Provider => _queryProvider;
}
