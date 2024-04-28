// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class QueryableExtensions {
    public static List<TItem> ToList<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, TItem>> project)
        => [.. IsNotNull(source).Select(project)];

    public static List<TResult> ToList<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, TResult>> project)
        => [.. IsNotNull(source).Select(project)];
}
