namespace DotNetToolbox.Data.Repositories;

public interface IAsyncOrderedQueryableRepository
    : IAsyncQueryableRepository;

public interface IAsyncOrderedQueryableRepository<TItem>
    : IAsyncQueryableRepository<TItem>
    , IAsyncOrderedQueryableRepository
    where TItem : class {

    IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);
}
