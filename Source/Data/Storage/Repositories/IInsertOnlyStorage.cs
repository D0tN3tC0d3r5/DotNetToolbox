namespace DotNetToolbox.Data.Repositories;

public interface IInsertOnlyStorage<TModel, in TKey>
    : IReadOnlyStorage<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    ValueTask Add(TModel input, CancellationToken ct = default);
    ValueTask Add(Action<TModel> setModel, CancellationToken ct = default);
}

public interface IInsertOnlyStorage<TModel>
    : IReadOnlyStorage<TModel>
    where TModel : class, IEntity, new() {
    ValueTask Add(TModel input, CancellationToken ct = default);
    ValueTask Add(Action<TModel> setModel, CancellationToken ct = default);
}
