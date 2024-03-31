namespace Sophia.Data;

public interface IRepository<TEntity>
    : IQueryable<TEntity>, IAsyncEnumerable<TEntity>
    where TEntity : class {
    Task Add(TEntity input, CancellationToken ct = default);
    Task Update(TEntity input, CancellationToken ct = default);
    Task Remove(TEntity input, CancellationToken ct = default);
}
