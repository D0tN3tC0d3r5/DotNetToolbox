namespace DotNetToolbox.Data.Repositories;

public interface IInsertOnlyRepository<TModel, TKey>
    : IReadOnlyRepository<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    Task Add(TModel input, CancellationToken ct = default);
    Task Create(Action<TModel> setModel, CancellationToken ct = default);
    Task<TKey> GenerateKey(CancellationToken ct = default);
}
