namespace DotNetToolbox.Data.Repositories;

public interface IWriteOnlyCompositeKeyEntityRepository<TRepository, TModel>
    : IInsertOnlyRepository<TRepository, TModel>
    where TRepository : IWriteOnlyCompositeKeyEntityRepository<TRepository, TModel>
    where TModel : class, ICompositeKeyEntity<TModel>, new() {
    Task Update(TModel input, CancellationToken ct = default);
    Task Update(object?[]? keys, Action<TModel> setModel, CancellationToken ct = default);
    Task Remove(object?[]? keys, CancellationToken ct = default);
}
