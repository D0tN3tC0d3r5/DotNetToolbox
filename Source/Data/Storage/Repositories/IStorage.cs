namespace DotNetToolbox.Data.Repositories;

#pragma warning disable CA1010
public interface IStorage
    : IOrderedQueryable {
    new IStorageProvider Provider { get; }
}
#pragma warning restore CA1010

public interface IStorage<out TModel>
    : IOrderedQueryable<TModel>,
      IAsyncEnumerable<TModel>,
      IStorage;

public interface IStorageProvider
    : IAsyncQueryProvider {
    IStorage Create(Expression expression);
    IStorage<TElement> Create<TElement>(Expression expression);
    new object? Execute(Expression expression);
    new TResult Execute<TResult>(Expression expression);
    new TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default);
    object? ExecuteAsync(Expression expression, CancellationToken cancellationToken = default);
}
