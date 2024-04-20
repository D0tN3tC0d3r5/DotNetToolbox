namespace DotNetToolbox.Data.Repositories;

public interface IUnitOfWorkRepository
    : IOrderedRepository {
    Task<int> SaveChanges(CancellationToken ct = default);
}

public interface IUnitOfWorkRepository<TItem>
    : IUnitOfWorkRepository
    , IOrderedRepository<TItem> {
}
