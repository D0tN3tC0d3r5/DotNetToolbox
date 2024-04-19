namespace DotNetToolbox.Data.Repositories;

public interface IOrderedQueryableRepository
    : IQueryableRepository;

public interface IOrderedQueryableRepository<TItem>
    : IQueryableRepository<TItem>,
      IOrderedQueryableRepository
    where TItem : class {

    IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);
}
