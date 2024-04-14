namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlyAsyncRepository<TItem>
    : IQueryable<TItem>,
      IAsyncEnumerable<TItem> {
    Task<bool> HaveAny(CancellationToken ct = default);
    Task<int> Count(CancellationToken ct = default);
    Task<TItem[]> ToArray(CancellationToken ct = default);
    Task<TItem?> GetFirst(CancellationToken ct = default);
}
