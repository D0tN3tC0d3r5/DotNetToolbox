namespace DotNetToolbox.Data.Repositories;

public interface IAsyncRepository;

public interface IAsyncRepository<TItem>
    : IAsyncQueryable<TItem>
    , IRepository<TItem>
    , IAsyncRepository {
    Task SeedAsync(IAsyncEnumerable<TItem> seed, CancellationToken ct = default);
    Task AddAsync(TItem newItem, CancellationToken ct = default);
    Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default);
    Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
}
