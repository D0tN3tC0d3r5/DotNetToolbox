namespace System.Linq.Async;

public class OrderedAsyncQueryable<TItem>(IEnumerable<TItem> source, Expression? expression = null)
    : AsyncQueryable<TItem>(source, expression), IOrderedAsyncQueryable<TItem>;
