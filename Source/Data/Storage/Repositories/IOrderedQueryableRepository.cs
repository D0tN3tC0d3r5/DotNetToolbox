namespace DotNetToolbox.Data.Repositories;

public interface IOrderedQueryableRepository<TItem>
    : IQueryableRepository<TItem>,
      IOrderedQueryable<TItem>
    where TItem : class;
