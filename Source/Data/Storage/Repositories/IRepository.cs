namespace DotNetToolbox.Data.Repositories;

public interface IRepository;

public interface IRepository<TItem>
    : IAsyncQueryable<TItem>
    , IRepository {

    #region Blocking

    void Seed(IEnumerable<TItem> seed);
    void Add(TItem newItem);
    void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem);
    void Remove(Expression<Func<TItem, bool>> predicate);

    #endregion

    #region Async

    Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default);
    Task AddAsync(TItem newItem, CancellationToken ct = default);
    Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default);
    Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    #endregion
}
