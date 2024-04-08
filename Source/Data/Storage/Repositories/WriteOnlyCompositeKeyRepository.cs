namespace DotNetToolbox.Data.Repositories;

public class WriteOnlyCompositeKeyRepository<TModel>
    : InsertOnlyRepository<TModel>,
      IWriteOnlyCompositeKeyRepository<TModel>
    where TModel : class, ICompositeKeyEntity<TModel>, new() {

    public virtual Task Update(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Update(object?[]? keys, Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Update(object key, Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task Remove(object?[]? keys, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Remove(object key, CancellationToken ct = default)
        => throw new NotImplementedException();
}
