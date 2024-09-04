namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static IOrderedAsyncQueryable<TItem> OrderBy<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector)
        => ((IQueryable<TItem>)source).OrderBy(fieldSelector).AsOrderedAsyncQueryable();
    public static IOrderedAsyncQueryable<TItem> OrderBy<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector, IComparer<TKey>? comparer)
        => ((IQueryable<TItem>)source).OrderBy(fieldSelector, comparer).AsOrderedAsyncQueryable();
    public static IOrderedAsyncQueryable<TItem> OrderByDescending<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector)
        => ((IQueryable<TItem>)source).OrderBy(fieldSelector).AsOrderedAsyncQueryable();
    public static IOrderedAsyncQueryable<TItem> OrderByDescending<TItem, TKey>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector, IComparer<TKey>? comparer)
        => ((IQueryable<TItem>)source).OrderBy(fieldSelector, comparer).AsOrderedAsyncQueryable();
    public static IOrderedAsyncQueryable<TItem> ThenBy<TItem, TKey>(this IOrderedAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector)
        => ((IOrderedQueryable<TItem>)source).ThenBy(fieldSelector).AsOrderedAsyncQueryable();
    public static IOrderedAsyncQueryable<TItem> ThenBy<TItem, TKey>(this IOrderedAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector, IComparer<TKey>? comparer)
        => ((IOrderedQueryable<TItem>)source).ThenBy(fieldSelector, comparer).AsOrderedAsyncQueryable();
    public static IOrderedAsyncQueryable<TItem> ThenByDescending<TItem, TKey>(this IOrderedAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector)
        => ((IOrderedQueryable<TItem>)source).ThenByDescending(fieldSelector).AsOrderedAsyncQueryable();
    public static IOrderedAsyncQueryable<TItem> ThenByDescending<TItem, TKey>(this IOrderedAsyncQueryable<TItem> source, Expression<Func<TItem, TKey>> fieldSelector, IComparer<TKey>? comparer)
        => ((IOrderedQueryable<TItem>)source).ThenByDescending(fieldSelector, comparer).AsOrderedAsyncQueryable();
}
