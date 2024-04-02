namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlySimpleKeyRepository<TRepository, TModel, in TKey>
    : IQueryableRepository<TRepository, TModel>
    where TRepository : IReadOnlySimpleKeyRepository<TRepository, TModel, TKey>
    where TModel : class, ISimpleKeyEntity<TModel, TKey>, new()
    where TKey : notnull {

    Task<int> CountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default);
    Task<bool> HaveAny(CancellationToken ct = default);
    Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
    Task<TModel?> FindByKey(TKey key, CancellationToken ct = default);
    Task<TModel?> FindFirst(CancellationToken ct = default);
    Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
}
