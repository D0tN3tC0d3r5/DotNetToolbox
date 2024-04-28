namespace DotNetToolbox.Data.Repositories;

public interface IUnitOfWorkRepository<TItem>
    : IRepository<TItem>{
    void SaveChanges();
}
