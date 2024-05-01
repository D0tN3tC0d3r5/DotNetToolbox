namespace DotNetToolbox.Data.Repositories.ValueObject;

public interface IUpdatableValueObjectRepository<TItem>
    : IAsyncQueryable<TItem>
    , IUpdatableRepository {

    #region Blocking

    void Seed(IEnumerable<TItem> seed);
    void Create(Action<TItem> setNewItem);
    void Add(TItem newItem);
    void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem);
    void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem);
    void Remove(Expression<Func<TItem, bool>> predicate);

    #endregion

    #region Async

    Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default);
    Task CreateAsync(Func<TItem, CancellationToken, Task> setNewItem, CancellationToken ct = default);
    Task AddAsync(TItem newItem, CancellationToken ct = default);
    Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default);
    Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default);
    Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    #endregion
}
