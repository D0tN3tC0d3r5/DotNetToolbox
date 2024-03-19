namespace DotNetToolbox.Extensions;
public static class StringBuilderExtensions {
    public static StringBuilder AppendIntoNewLine(this StringBuilder builder, string? text)
        => builder.AppendIntoNewLine(string.Empty, text);

    public static StringBuilder AppendIntoNewLine(this StringBuilder builder, string ident, string? text)
        => (string.IsNullOrWhiteSpace(text) || builder.Length == 0
               ? builder
               : builder.AppendLine())
           .Append($"{ident}{text}");
}
