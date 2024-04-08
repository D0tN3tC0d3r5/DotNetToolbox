namespace DotNetToolbox.Data.Repositories;

public class ModelQueryable<TModel>(IAsyncQueryProvider queryProvider, Expression expression)
    : IOrderedQueryable<TModel>,
      IAsyncEnumerable<TModel> {
    public virtual Type ElementType
        => typeof(TModel);
    public virtual Expression Expression { get; } = expression;
    public virtual IQueryProvider Provider => queryProvider;
    public virtual IEnumerator<TModel> GetEnumerator()
        => queryProvider.Execute<IEnumerable<TModel>>(Expression).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
        => queryProvider.Execute<IEnumerable>(Expression).GetEnumerator();
    public virtual IAsyncEnumerator<TModel> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => queryProvider
          .ExecuteAsync<IAsyncEnumerable<TModel>>(Expression, cancellationToken)
          .GetAsyncEnumerator(cancellationToken);
}
