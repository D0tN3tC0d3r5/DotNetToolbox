namespace Sophia.Data.Helpers;

internal sealed class StringArrayItemComparer()
    : ValueComparer<string>((a, b) => a != null && a.Equals(b, OrdinalIgnoreCase),
                            s => s.GetHashCode());