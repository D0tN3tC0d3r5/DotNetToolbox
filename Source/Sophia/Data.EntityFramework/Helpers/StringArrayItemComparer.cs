namespace Sophia.Data.Helpers;

internal class StringArrayItemComparer()
    : ValueComparer<string>((a, b) => a != null && a.Equals(b, StringComparison.InvariantCultureIgnoreCase),
                            s => s.GetHashCode());