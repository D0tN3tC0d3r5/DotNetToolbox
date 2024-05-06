namespace DotNetToolbox.Data.Repositories;

public interface IQueryableRepository
    : IAsyncQueryable;

public interface IQueryableRepository<out TItem>
    : IQueryableRepository,
      IAsyncQueryable<TItem>;
