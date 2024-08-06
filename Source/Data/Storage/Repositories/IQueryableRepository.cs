namespace DotNetToolbox.Data.Repositories;

[SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "Implemented below")]
public interface IQueryableRepository
    : IAsyncQueryable;

public interface IQueryableRepository<out TItem>
    : IQueryableRepository,
      IAsyncQueryable<TItem>;
