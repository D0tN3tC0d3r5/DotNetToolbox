namespace DotNetToolbox.Data.Repositories;

public interface IAsyncItemSet<TItem>
    : IItemSet<TItem, InMemoryAsyncRepositoryStrategy<TItem>>;

public interface IAsyncOrderedItemSet<TItem>
    : IOrderedQueryable<TItem>,
      IItemSet<TItem, InMemoryAsyncRepositoryStrategy<TItem>>;

public interface IAsyncItemSet<out TItem, out TStrategy>
    : IItemSet<TItem, TStrategy>
    where TStrategy : IAsyncQueryStrategy<TStrategy> {
    new TStrategy Strategy { get; }
}

public interface IAsyncOrderedItemSet<out TItem, out TStrategy>
    : IOrderedQueryable<TItem>,
      IItemSet<TItem, TStrategy>
    where TStrategy : IAsyncQueryStrategy<TStrategy>;
