namespace DotNetToolbox.Data.Repositories;

public interface IAsyncUnitOfWorkRepository
    : IAsyncOrderedRepository {
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface IAsyncUnitOfWorkRepository<TItem>
    : IAsyncUnitOfWorkRepository
    , IAsyncOrderedRepository<TItem>
    where TItem : class {
}
