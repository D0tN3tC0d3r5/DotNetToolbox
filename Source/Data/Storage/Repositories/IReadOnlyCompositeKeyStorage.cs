namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlyCompositeKeyStorage<TModel>
    : IStorage<TModel>
    where TModel : class, ICompositeKeyEntity<TModel>, new() {

    Task<int> CountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default);
    Task<bool> HaveAny(CancellationToken ct = default);
    Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
    Task<TModel?> FindByKey(object key, CancellationToken ct = default);
    Task<TModel?> FindByKeys(object?[] keys, CancellationToken ct = default);
    Task<TModel?> FindFirst(CancellationToken ct = default);
    Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
}
