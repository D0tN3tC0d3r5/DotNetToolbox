namespace DotNetToolbox.Data.Repositories;

public interface IOrderedRepository<TItem>
    : IRepository<TItem>,
      IOrderedQueryable<TItem>;
