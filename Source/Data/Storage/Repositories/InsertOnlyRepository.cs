namespace DotNetToolbox.Data.Repositories;

public class InsertOnlyRepository<TModel, TKey>
    : ReadOnlyStorage<TModel, TKey>,
      IInsertOnlyStorage<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    public virtual ValueTask Add(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual ValueTask Add(Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();
}

public class InsertOnlyRepository<TModel>
    : ReadOnlyStorage<TModel>,
      IInsertOnlyStorage<TModel>
    where TModel : class, IEntity, new() {
    public virtual ValueTask Add(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual ValueTask Add(Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();
}
