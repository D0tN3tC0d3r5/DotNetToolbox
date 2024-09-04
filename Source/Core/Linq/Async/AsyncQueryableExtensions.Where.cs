namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static IAsyncQueryable<TItem> Where<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate)
        => ((IQueryable<TItem>)source).Where(predicate).AsAsyncQueryable();
}
