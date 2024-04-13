namespace DotNetToolbox.Data.Repositories;

#pragma warning disable CA1010
public interface IItemSet
    : IQueryable;
#pragma warning restore CA1010

public interface IItemSet<TItem>
    : IItemSet<TItem, InMemoryRepositoryStrategy<TItem>>;

public interface IOrderedItemSet<TItem>
    : IOrderedQueryable<TItem>,
      IItemSet<TItem>;

public interface IItemSet<out TItem, out TStrategy>
    : IQueryable<TItem>,
      IItemSet
    where TStrategy : IQueryStrategy {
    TStrategy Strategy { get; }
}

public interface IOrderedItemSet<out TItem, out TStrategy>
    : IOrderedQueryable<TItem>,
      IItemSet<TItem, TStrategy>
    where TStrategy : IQueryStrategy;
