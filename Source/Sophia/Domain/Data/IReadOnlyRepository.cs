namespace Sophia.Data;

public interface IReadOnlyRepository<[DynamicallyAccessedMembers(IEntity.AccessedMembers)] TModel, in TKey>
    : IQueryable<TModel>,
      IAsyncEnumerable<TModel>
    where TModel : class
    where TKey : notnull {

    Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default);
    Task<bool> HaveAny(CancellationToken ct = default);
    Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
    Task<TModel?> FindByKey(TKey key, CancellationToken ct = default);
    Task<TModel?> FindFirst(CancellationToken ct = default);
    Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
}
