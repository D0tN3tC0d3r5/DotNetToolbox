namespace DotNetToolbox.Data.Repositories;

public interface IUnitOfWorkRepository
    : IRepository {
    void SaveChanges();
}

public interface IUnitOfWorkRepository<TItem>
    : IUnitOfWorkRepository
    , IRepository<TItem>{
}
