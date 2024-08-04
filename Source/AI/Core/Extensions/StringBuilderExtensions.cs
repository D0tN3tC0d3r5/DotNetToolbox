namespace DotNetToolbox.AI.Extensions;

internal static class StringBuilderExtensions {
    public static void AppendSection(this StringBuilder builder, IContextSection section)
        => AppendSection(builder, string.Empty, section);

    public static void AppendSection(this StringBuilder builder, string indentation, IContextSection section) {
        var text = section.GetIndentedText(indentation);
        if (string.IsNullOrWhiteSpace(text))
            return;
        builder.AppendIntoNewLine($"{indentation}{section.Title}");
        builder.AppendIntoNewLine(text);
    }

    public static void AppendSection(this StringBuilder builder, IReadOnlyCollection<string> items, string? title = null)
        => AppendSection(builder, string.Empty, items, title);

    public static void AppendSection(this StringBuilder builder, string indentation, IReadOnlyCollection<string> items, string? title = null) {
        if (items.Count == 0)
            return;
        if (!string.IsNullOrWhiteSpace(title))
            builder.AppendIntoNewLine($"{indentation}{title}");
        foreach (var item in items)
            builder.AppendIntoNewLine($"{indentation}{item}");
    }
}
