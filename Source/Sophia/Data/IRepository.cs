namespace Sophia.Data;

public interface IRepository<TEntity>
    : IQueryable<TEntity>, IAsyncEnumerable<TEntity>
    where TEntity : class {
    Task Add(TEntity input, CancellationToken cancellationToken = default);
    Task Update(TEntity input, CancellationToken cancellationToken = default);
    Task Remove(TEntity input, CancellationToken cancellationToken = default);
}
