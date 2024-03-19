namespace DotNetToolbox.AI.Extensions;
internal static class StringBuilderExtensions {
    public static void AppendSection<TItem>(this StringBuilder builder, ICollection<TItem> items, [CallerArgumentExpression(nameof(items))]string? paramName = null)
        => builder.AppendSection(string.Empty, items, paramName);

    public static void AppendSection<TItem>(this StringBuilder builder, string ident, ICollection<TItem> items, [CallerArgumentExpression(nameof(items))] string? paramName = null) {
        if (items.Count == 0) return;
        builder.AppendIntoNewLine($"{ident}{paramName}");
        foreach (var item in items)
            builder.AppendIntoNewLine($"{ident}{item}");
    }
}
