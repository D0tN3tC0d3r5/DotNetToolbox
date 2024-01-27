using DotNetToolbox.ConsoleApplication.Application;

namespace DotNetToolbox.ConsoleApplication.Utilities;

internal class OutputFormatter {
    public static string FormatException(Exception exception) {
        var builder = new StringBuilder();
        ShowException(builder, exception);
        return builder.ToString();
    }

    public static string FormatValidationErrors(IReadOnlyCollection<ValidationError> errors) {
        var builder = new StringBuilder();
        foreach (var error in errors)
            builder.AppendLine($"Validation error: {error.Message}");
        return builder.ToString();
    }

    public static string FormatHelp(IHasChildren node, bool includeApplication) {
        var path = node.GetPath(includeApplication);

        var builder = new StringBuilder();
        ShowDescription(builder, node);
        builder.AppendLine();
        ShowUsage(builder, path, node);
        ShowItems(builder, "Options", node.Options);
        ShowItems(builder, "Parameters", node.Parameters);
        ShowItems(builder, "Commands", node.Commands);
        return builder.ToString();
    }

    private static void ShowDescription(StringBuilder builder, INode node) {
        if (node is IApplication app) builder.AppendLine(app.FullName);
        if (string.IsNullOrWhiteSpace(node.Description)) return;
        builder.AppendLine(node.Description);
        builder.AppendLine();
    }

    private static void ShowUsage(StringBuilder builder, string? path, IHasChildren node) {
        if (string.IsNullOrEmpty(path)) return;
        builder.Append("Usage: ").Append(path);
        if (node.Options.Length != 0) builder.Append(" [Options]");
        if (node.Parameters.Length != 0) builder.Append(" [Parameters]");
        if (node.Commands.Length != 0) builder.Append(" [Commands]");
        builder.AppendLine();
    }

    private static void ShowItems(StringBuilder builder, string section, IReadOnlyCollection<INode> options) {
        if (options.Count == 0) return;
        builder.AppendLine($"{section}:");
        foreach (var option in options)
            option.AppendHelp(builder);
        builder.AppendLine();
    }

    private static void ShowException(StringBuilder builder, Exception ex) {
        ShowDescription(builder, ex, false, 0);
        ShowStackTrace(builder, ex, 1);
        var indent = 1;
        while (ex.InnerException is not null) {
            ex = ex.InnerException;
            ShowDescription(builder, ex, true, indent);
            ShowStackTrace(builder, ex, indent + 1);
            indent++;
        }
    }

    private static void ShowDescription(StringBuilder builder, Exception ex, bool isInner, int indent) {
        builder.Append(' ', indent * 4);
        if (isInner) builder.Append("Inner Exception => ");
        builder.Append(ex.GetType().Name);
        builder.Append(": ");
        builder.AppendLine(ex.Message);
    }

    private static void ShowStackTrace(StringBuilder builder, Exception ex, int indent) {
        builder.Append(' ', indent * 4).AppendLine("Stack Trace:");
        var lines = ex.StackTrace?.Split(Environment.NewLine) ?? [];
        _ = lines.Aggregate(builder, (s, l) => s.Append(' ', (indent + 1) * 4).AppendLine(l));
    }
}
