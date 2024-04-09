namespace DotNetToolbox.Data.Repositories;

public interface ISimpleKeyStorage<TModel, in TKey>
    : IInsertOnlyStorage<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    Task Update(TModel input, CancellationToken ct = default);
    Task Update(TKey key, Action<TModel> setModel, CancellationToken ct = default);
    Task Remove(TKey key, CancellationToken ct = default);
}
