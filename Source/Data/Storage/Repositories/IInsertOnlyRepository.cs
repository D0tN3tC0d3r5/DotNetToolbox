namespace DotNetToolbox.Data.Repositories;

public interface IInsertOnlyRepository<TModel>
    where TModel : class, new() {
    ValueTask Add(TModel input, CancellationToken ct = default);
    ValueTask Add(Action<TModel> setModel, CancellationToken ct = default);
}
