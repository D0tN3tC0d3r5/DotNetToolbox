// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class QueryableExtensions {
    public static TItem[] ToArray<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, TItem>> project)
        => [.. IsNotNull(source).Select(project)];
    public static TResult[] ToArray<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, TResult>> project)
        => [.. IsNotNull(source).Select(project)];
}
