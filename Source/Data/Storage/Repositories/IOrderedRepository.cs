namespace DotNetToolbox.Data.Repositories;

public interface IOrderedRepository<TItem>
    : IRepository<TItem>,
      IOrderedQueryable<TItem>
    where TItem : class;
