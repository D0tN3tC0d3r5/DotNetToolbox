namespace DotNetToolbox.Data.Repositories;

#pragma warning disable CA1010
public interface IQueryableSet
    : IOrderedQueryable {
    new IReadOnlyStorageProvider Provider { get; }
}
#pragma warning restore CA1010

public interface IQueryableSet<out TModel>
    : IOrderedQueryable<TModel>,
      IAsyncEnumerable<TModel>,
      IQueryableSet;
