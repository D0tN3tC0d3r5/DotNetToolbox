namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlyRepository<TEntity>
    : IItemSet<TEntity> {
    Task<TEntity[]> GetList(CancellationToken ct = default);
    Task<int> Count(CancellationToken ct = default);
    Task<int> Count(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
    Task<bool> HaveAny(CancellationToken ct = default);
    Task<bool> HaveAny(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
    Task<TEntity?> FindFirst(CancellationToken ct = default);
    Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
}
