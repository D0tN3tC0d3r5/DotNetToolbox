namespace DotNetToolbox.Data.Repositories;

public class Storage<TModel>(StorageProvider queryProvider, Expression expression)
    : IStorage<TModel> {
    public Type ElementType { get; } = typeof(TModel);
    public IAsyncEnumerator<TModel> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => queryProvider
            .ExecuteAsync<IAsyncEnumerable<TModel>>(Expression, cancellationToken)
            .GetAsyncEnumerator(cancellationToken);
    public IEnumerator<TModel> GetEnumerator()
        => queryProvider.Execute<IEnumerable<TModel>>(Expression).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public virtual Expression Expression { get; } = expression;
    IQueryProvider IQueryable.Provider => Provider;
    public IStorageProvider Provider => queryProvider;
}
