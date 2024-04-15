namespace DotNetToolbox.Data.Repositories;

public interface IAsyncRepository
    : IAsyncRepository<object>;

public interface IAsyncRepository<TItem>
    : IQueryable<TItem>,
      IAsyncEnumerable<TItem>
    where TItem : class {
    new IAsyncRepositoryStrategy<TItem> Provider { get; }
    Task<bool> HaveAny(CancellationToken ct = default);
    Task<int> Count(CancellationToken ct = default);
    Task<TItem[]> ToArray(CancellationToken ct = default);
    Task<TItem?> GetFirst(CancellationToken ct = default);
    Task Add(TItem newItem, CancellationToken ct = default);
    Task Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default);
    Task Remove(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
}
