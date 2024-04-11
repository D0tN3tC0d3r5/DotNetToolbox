namespace DotNetToolbox.Data.Repositories;

public interface IAsyncRepository<TItem, out TStrategy>
    : IItemSet<TItem, TStrategy>
    where TStrategy : IAsyncRepositoryStrategy<TStrategy> {
    Task<TItem[]> GetList(CancellationToken ct = default);
    Task<int> Count(CancellationToken ct = default);
    Task<int> CountWhere(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<bool> HaveAny(CancellationToken ct = default);
    Task<bool> HaveAnyWhere(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
    Task<TItem?> FindFirst(CancellationToken ct = default);
    Task<TItem?> FindFirstWhere(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
}
