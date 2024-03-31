namespace Sophia.Data;

public enum SortDirection {
    Ascending,
    Descending,
}

public record SortClause(string PropertyPath, SortDirection Direction);
public record IncludeClause(string PropertyPath);

public interface IRepository<TModel, in TKey>
    where TModel : class
    where TKey : notnull {
    DataContext Context { get; }
    Task<TModel> CreateDefault(CancellationToken ct = default);

    Task<IReadOnlyList<TModel>> GetList(CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, List<IncludeClause>>> include, CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, List<SortClause>>> sortBy, CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, bool>> filterBy, CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, List<SortClause>>> sortBy, Expression<Func<TModel, List<IncludeClause>>> include, CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, bool>> filterBy, Expression<Func<TModel, List<IncludeClause>>> include, CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, bool>> filterBy, Expression<Func<TModel, List<SortClause>>> sortBy, CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, bool>> filterBy, Expression<Func<TModel, List<SortClause>>> sortBy, Expression<Func<TModel, List<IncludeClause>>> include, CancellationToken ct = default);
    Task<bool> HasAny(CancellationToken ct = default);
    Task<bool> HasAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);
    Task<TModel?> FindFirst(CancellationToken ct = default);
    Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default);

    Task Add(TModel input, CancellationToken ct = default);
    Task Add(Action<TModel> setModel, CancellationToken ct = default);
    Task Update(TModel input, CancellationToken ct = default);
    Task Update(TKey key, Action<TModel> setModel, CancellationToken ct = default);
    Task Remove(TKey key, CancellationToken ct = default);
}
