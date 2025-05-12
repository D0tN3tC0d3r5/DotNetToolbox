namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static IAsyncOrderedQueryable<TItem> OrderBy<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector)
        => ((IQueryable<TItem>)source).OrderBy(fieldSelector).AsAsyncOrderedQueryable();
    public static IAsyncOrderedQueryable<TItem> OrderBy<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector, IComparer<TKey>? comparer)
        => ((IQueryable<TItem>)source).OrderBy(fieldSelector, comparer).AsAsyncOrderedQueryable();
    public static IAsyncOrderedQueryable<TItem> OrderByDescending<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector)
        => ((IQueryable<TItem>)source).OrderBy(fieldSelector).AsAsyncOrderedQueryable();
    public static IAsyncOrderedQueryable<TItem> OrderByDescending<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector, IComparer<TKey>? comparer)
        => ((IQueryable<TItem>)source).OrderBy(fieldSelector, comparer).AsAsyncOrderedQueryable();
    public static IAsyncOrderedQueryable<TItem> ThenBy<TItem, TKey>(this IAsyncOrderedQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector)
        => ((IOrderedQueryable<TItem>)source).ThenBy(fieldSelector).AsAsyncOrderedQueryable();
    public static IAsyncOrderedQueryable<TItem> ThenBy<TItem, TKey>(this IAsyncOrderedQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector, IComparer<TKey>? comparer)
        => ((IOrderedQueryable<TItem>)source).ThenBy(fieldSelector, comparer).AsAsyncOrderedQueryable();
    public static IAsyncOrderedQueryable<TItem> ThenByDescending<TItem, TKey>(this IAsyncOrderedQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector)
        => ((IOrderedQueryable<TItem>)source).ThenByDescending(fieldSelector).AsAsyncOrderedQueryable();
    public static IAsyncOrderedQueryable<TItem> ThenByDescending<TItem, TKey>(this IAsyncOrderedQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector, IComparer<TKey>? comparer)
        => ((IOrderedQueryable<TItem>)source).ThenByDescending(fieldSelector, comparer).AsAsyncOrderedQueryable();
}
