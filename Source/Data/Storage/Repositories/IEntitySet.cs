namespace DotNetToolbox.Data.Repositories;

#pragma warning disable CA1010
public interface IEntitySet
    : IQueryable {
    IRepositoryStrategy Strategy { get; }
}
#pragma warning restore CA1010

public interface IEntitySet<out TEntity>
    : IQueryable<TEntity>,
      IAsyncEnumerable<TEntity>,
      IEntitySet;

public interface IOrderedEntitySet<out TEntity>
    : IOrderedQueryable<TEntity>,
      IEntitySet<TEntity>;
