namespace DotNetToolbox.Data.Repositories;

public interface IInsertOnlyRepository<TModel, in TKey>
    : IReadOnlyRepository<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    Task Add(TModel input, CancellationToken ct = default);
    Task Create(Action<TModel> setModel, CancellationToken ct = default);
}

public interface IInsertOnlyRepository<TModel>
    : IReadOnlyRepository<TModel>
    where TModel : class, IEntity, new() {
    Task Add(TModel input, CancellationToken ct = default);
    Task Create(Action<TModel> setModel, CancellationToken ct = default);
}
