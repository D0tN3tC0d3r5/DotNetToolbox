namespace DotNetToolbox.Data.Repositories;

public interface IUnitOfWorkRepository
    : IRepository {
    Task<int> SaveChanges(CancellationToken ct = default);
}

public interface IUnitOfWorkRepository<TItem>
    : IUnitOfWorkRepository
    , IRepository<TItem>{
}
