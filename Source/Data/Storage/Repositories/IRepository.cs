namespace DotNetToolbox.Data.Repositories;

public interface IRepository<TEntity>
    : IInsertOnlyRepository<TEntity> {
    Task Update(TEntity input, CancellationToken ct = default);
    Task<TEntity?> Patch(Expression<Func<TEntity, bool>> predicate, Action<TEntity> setModel, CancellationToken ct = default);
    Task<TEntity> PatchOrCreate(Expression<Func<TEntity, bool>> predicate, Action<TEntity> setModel, CancellationToken ct = default);
    Task Remove(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
}
