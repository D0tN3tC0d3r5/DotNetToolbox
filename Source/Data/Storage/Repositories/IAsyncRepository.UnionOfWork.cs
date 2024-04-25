namespace DotNetToolbox.Data.Repositories;

public interface IAsyncUnitOfWorkRepository
    : IAsyncRepository {
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface IAsyncUnitOfWorkRepository<TItem>
    : IAsyncUnitOfWorkRepository
    , IAsyncRepository<TItem>{
}
