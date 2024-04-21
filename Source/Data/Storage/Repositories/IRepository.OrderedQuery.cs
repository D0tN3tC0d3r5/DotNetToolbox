namespace DotNetToolbox.Data.Repositories;

public interface IOrderedRepository
    : IRepository;

public interface IOrderedRepository<TItem>
    : IRepository<TItem>
    , IOrderedRepository
    where TItem : class {
    IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);
}
