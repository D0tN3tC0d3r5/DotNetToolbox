namespace Sophia.Data;

public interface IWriteOnlyRepository<[DynamicallyAccessedMembers(IEntity.AccessedMembers)] TModel, in TKey>
    where TModel : class
    where TKey : notnull {
    Task Add(TModel input, CancellationToken ct = default);
    Task Add(Action<TModel> setModel, CancellationToken ct = default);
    Task Update(TModel input, CancellationToken ct = default);
    Task Update(TKey key, Action<TModel> setModel, CancellationToken ct = default);
    Task Remove(TKey key, CancellationToken ct = default);
}
