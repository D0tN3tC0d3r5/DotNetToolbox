namespace DotNetToolbox.Data.Repositories;

public interface IWriteOnlySimpleKeyEntityRepository<TRepository, TModel, in TKey>
    : IInsertOnlyRepository<TRepository, TModel>
    where TRepository : IWriteOnlySimpleKeyEntityRepository<TRepository, TModel, TKey>
    where TModel : class, ISimpleKeyEntity<TModel, TKey>, new()
    where TKey : notnull {
    Task Update(TModel input, CancellationToken ct = default);
    Task Update(TKey key, Action<TModel> setModel, CancellationToken ct = default);
    Task Remove(TKey key, CancellationToken ct = default);
}
