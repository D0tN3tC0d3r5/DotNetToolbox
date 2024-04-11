namespace DotNetToolbox.Data.Repositories;

public interface IInsertOnlyRepository<TEntity>
    : IReadOnlyRepository<TEntity> {
    Task Add(TEntity input, CancellationToken ct = default);
    Task AddOrUpdate(TEntity input, CancellationToken ct = default);
    Task<TEntity> Create(Action<TEntity> setModel, CancellationToken ct = default);
    Task<object> GenerateKey(CancellationToken ct = default);
}
