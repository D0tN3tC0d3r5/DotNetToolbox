namespace DotNetToolbox.Data.Repositories;

public interface IRepository<TModel>
    : IInsertOnlyRepository<TModel>
    where TModel : class, IEntity, new() {
    Task Update(TModel input, CancellationToken ct = default);
    Task AddOrUpdate(TModel input, CancellationToken ct = default);
    Task Patch<TKey>(TKey key, Action<TModel> setModel, CancellationToken ct = default)
        where TKey : notnull;
    Task CreateOrPatch<TKey>(TKey key, Action<TModel> setModel, CancellationToken ct = default)
        where TKey : notnull;
    Task Remove<TKey>(TKey key, CancellationToken ct = default)
        where TKey : notnull;
}

public interface IRepository<TModel, in TKey>
    : IInsertOnlyRepository<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    Task Update(TModel input, CancellationToken ct = default);
    Task AddOrUpdate(TModel input, CancellationToken ct = default);
    Task Patch(TKey key, Action<TModel> setModel, CancellationToken ct = default);
    Task CreateOrPatch(TKey key, Action<TModel> setModel, CancellationToken ct = default);
    Task Remove(TKey key, CancellationToken ct = default);
}
