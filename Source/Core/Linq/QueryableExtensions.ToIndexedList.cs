// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static partial class QueryableExtensions {
    public static List<IndexedItem<TItem>> ToIndexedList<TItem>(this IQueryable<TItem> source)
        => source.ToIndexedList(i => i);
    public static List<IndexedItem<TOutput>> ToIndexedList<TItem, TOutput>(this IQueryable<TItem> source, Func<TItem, TOutput> transform) {
        using var enumerator = source.GetEnumerator();
        var list = new List<IndexedItem<TOutput>>();
        var index = 0;
        var hasNext = enumerator.MoveNext();
        while (hasNext) {
            var value = transform(enumerator.Current);
            hasNext = enumerator.MoveNext();
            list.Add(new(index++, value, !hasNext));
        }
        return list;
    }
}
