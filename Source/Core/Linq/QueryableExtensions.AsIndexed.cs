// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class QueryableExtensions {
    public static IQueryable<Indexed<TItem>> AsIndexed<TItem>(this IQueryable source) => source.Cast<TItem>().AsIndexed();

    public static IQueryable<Indexed<TOutput>> AsIndexed<TItem, TOutput>(this IQueryable source, Expression<Func<TItem, TOutput>> transform) => source.Cast<TItem>().AsIndexed(transform);

    public static IQueryable<Indexed<TItem>> AsIndexed<TItem>(this IQueryable<TItem> source) => source.AsIndexed(i => i);

    public static IQueryable<Indexed<TOutput>> AsIndexed<TItem, TOutput>(this IQueryable<TItem> source, Expression<Func<TItem, TOutput>> transform) => source.Select((v, i) => new Indexed<TOutput>(i, transform.Compile()(v)));
}
