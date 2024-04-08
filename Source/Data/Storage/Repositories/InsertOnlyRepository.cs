namespace DotNetToolbox.Data.Repositories;

public class InsertOnlyRepository<TModel>
    : IInsertOnlyRepository<TModel>
    where TModel : class, new() {

    public virtual ValueTask Add(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual ValueTask Add(Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();
}
