namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlyRepository<TModel, in TKey>
    : IEntitySet<TModel>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    Task<IReadOnlyList<TModel>> GetListAsync(CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<int> CountAsync(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
    Task<bool> HaveAny(CancellationToken ct = default);
    Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
    Task<TModel?> FindFirst(CancellationToken ct = default);
    Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
    Task<TModel?> FindByKey(TKey key, CancellationToken ct = default);
}
