namespace DotNetToolbox.Data.Repositories;

public interface IAsyncUnitOfWorkRepository<TItem>
    : IAsyncRepository<TItem>
    , IUnitOfWorkRepository<TItem> {
    Task SaveChangesAsync(CancellationToken ct = default);
}
