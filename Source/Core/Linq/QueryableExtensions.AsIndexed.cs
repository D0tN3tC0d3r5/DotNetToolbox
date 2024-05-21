// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class QueryableExtensions {
    public static IQueryable<Indexed<TItem>> AsIndexed<TItem>(this IQueryable source)
        => source.Cast<TItem>().AsIndexed();

    public static IQueryable<Indexed<TItem>> AsIndexed<TItem>(this IQueryable<TItem> source)
        => source.Select((x, i) => new Indexed<TItem>(i, x));
}
