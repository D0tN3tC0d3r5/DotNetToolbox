namespace DotNetToolbox.Data.Repositories;

public interface IQueryableRepository
    : IQueryable;

public interface IQueryableRepository<out TItem>
    : IQueryableRepository
    , IAsyncQueryable<TItem>;

