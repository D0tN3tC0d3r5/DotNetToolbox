namespace DotNetToolbox.Data.Repositories;

public interface IAsyncOrderedQueryableRepository
    : IAsyncQueryableRepository;

public interface IAsyncOrderedQueryableRepository<TItem>
    : IAsyncQueryableRepository<TItem>
    , IAsyncOrderedQueryableRepository
    where TItem : class {

    AsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    AsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    AsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector);

    AsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);
}
