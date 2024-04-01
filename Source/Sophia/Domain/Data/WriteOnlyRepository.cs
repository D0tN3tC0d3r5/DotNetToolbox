namespace Sophia.Data;

public class WriteOnlyRepository<[DynamicallyAccessedMembers(IEntity.AccessedMembers)] TModel, TKey>
    : IWriteOnlyRepository<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {

    public virtual Task Add(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Add(Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Update(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Update(TKey key, Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Remove(TKey key, CancellationToken ct = default)
        => throw new NotImplementedException();
}
