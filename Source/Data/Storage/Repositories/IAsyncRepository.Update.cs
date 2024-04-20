namespace DotNetToolbox.Data.Repositories;

public partial interface IAsyncRepository<TItem> {
    Task AddAsync(TItem newItem, CancellationToken ct = default);
    Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default);
    Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);
}
