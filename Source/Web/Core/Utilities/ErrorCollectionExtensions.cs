namespace WebApi.Utilities;

public static class ErrorCollectionExtensions {
    public static Dictionary<string, string[]> GroupedBySource(this IEnumerable<Error> errors)
        => errors.SelectMany(e => e.Sources.Select(s => new { Source = s, e.Message }))
                     .GroupBy(x => x.Source)
                     .ToDictionary(g => g.Key, g => g.ToArray(x => x.Message), StringComparer.Ordinal);
}
