namespace DotNetToolbox.Data.Repositories;

#pragma warning disable CA1010
public interface IItemSet
    : IQueryable {
    IRepositoryStrategy Strategy { get; }
}
#pragma warning restore CA1010

public interface IItemSet<out TItem>
    : IQueryable<TItem>,
      IAsyncEnumerable<TItem>,
      IItemSet;

public interface IOrderedItemSet<out TItem>
    : IOrderedQueryable<TItem>,
      IItemSet<TItem>;
