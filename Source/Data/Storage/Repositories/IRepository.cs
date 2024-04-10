namespace DotNetToolbox.Data.Repositories;

public interface IRepository<TModel, TKey>
    : IInsertOnlyRepository<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    Task Update(TModel input, CancellationToken ct = default);
    Task AddOrUpdate(TModel input, CancellationToken ct = default);
    Task Patch(Func<CancellationToken, Task<TModel?>> find, Action<TModel> setModel, CancellationToken ct = default);
    Task Patch(TKey key, Action<TModel> setModel, CancellationToken ct = default);
    Task CreateOrPatch(Func<CancellationToken, Task<TModel?>> find, Action<TModel> setModel, CancellationToken ct = default);
    Task CreateOrPatch(TKey key, Action<TModel> setModel, CancellationToken ct = default);
    Task Remove(Func<CancellationToken, Task<TModel?>> find, CancellationToken ct = default);
    Task Remove(TKey key, CancellationToken ct = default);
}
