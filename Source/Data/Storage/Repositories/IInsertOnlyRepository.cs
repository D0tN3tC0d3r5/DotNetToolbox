namespace DotNetToolbox.Data.Repositories;

public interface IInsertOnlyRepository<TRepository, TModel>
    where TRepository : IInsertOnlyRepository<TRepository, TModel>
    where TModel : class, new() {
    ValueTask Add(TModel input, CancellationToken ct = default);
    ValueTask Add(Action<TModel> setModel, CancellationToken ct = default);
}
