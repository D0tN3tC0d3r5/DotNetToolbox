namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlyRepository<TModel, in TKey>
    : IEntitySet<TModel>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {

    Task<int> CountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default);
    Task<bool> HaveAny(CancellationToken ct = default);
    Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
    Task<TModel?> FindByKey(TKey key, CancellationToken ct = default);
    Task<TModel?> FindFirst(CancellationToken ct = default);
    Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
}

public interface IReadOnlyRepository<TModel>
    : IEntitySet<TModel>
    where TModel : class, IEntity, new() {

    Task<int> CountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default);
    Task<bool> HaveAny(CancellationToken ct = default);
    Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
    Task<TModel?> FindByKey<TKey>(TKey key, CancellationToken ct = default)
        where TKey : notnull;
    Task<TModel?> FindFirst(CancellationToken ct = default);
    Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
}
