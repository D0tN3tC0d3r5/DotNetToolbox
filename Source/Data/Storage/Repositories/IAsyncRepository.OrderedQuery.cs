namespace DotNetToolbox.Data.Repositories;

public interface IAsyncOrderedRepository
    : IAsyncRepository;

public interface IAsyncOrderedRepository<TItem>
    : IAsyncRepository<TItem>
    , IAsyncOrderedRepository {
    IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);
}
