namespace DotNetToolbox.Data.Repositories;

public class WriteOnlySimpleKeyEntityRepository<TRepository, TModel, TKey>
    : InsertOnlyRepository<TRepository, TModel>,
      IWriteOnlySimpleKeyEntityRepository<TRepository, TModel, TKey>
    where TRepository : WriteOnlySimpleKeyEntityRepository<TRepository, TModel, TKey>
    where TModel : class, ISimpleKeyEntity<TModel, TKey>, new()
    where TKey : notnull {
    public virtual Task Update(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Update(TKey key, Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Remove(TKey key, CancellationToken ct = default)
        => throw new NotImplementedException();
}
