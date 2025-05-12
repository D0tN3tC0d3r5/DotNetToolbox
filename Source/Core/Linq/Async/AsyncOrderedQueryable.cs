namespace System.Linq.Async;

public class AsyncOrderedQueryable<TItem>(IEnumerable<TItem> source, Expression? expression = null)
    : AsyncQueryable<TItem>(source, expression)
    , IAsyncOrderedQueryable<TItem>;
