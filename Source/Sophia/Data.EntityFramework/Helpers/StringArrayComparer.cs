namespace Sophia.Data.Helpers;

internal sealed class StringArrayComparer()
    : ValueComparer<string[]>((a, b) => a != null && b != null && a.SequenceEqual(b, new StringArrayItemComparer()),
                              s => s.Aggregate(0, HashCode.Combine));
