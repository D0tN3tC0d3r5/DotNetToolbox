namespace DotNetToolbox.ConsoleApplication.Utilities;

internal class OutputFormatter {
    private const int _indentSize = 4;

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

    public static string FormatHelp(IHasChildren node) {
        var builder = new StringBuilder();
        ShowNodeDescription(builder, node);
        ShowUsage(builder, node);
        ShowAliases(builder, node);
        ShowItems(builder, "Options", node.Options);
        ShowItems(builder, "Parameters", node.Parameters);
        ShowItems(builder, "Commands", node.Commands);
        return builder.ToString();
    }

    private static void ShowNodeDescription(StringBuilder builder, INode node) {
        if (node is IApplication app) builder.AppendLine(app.FullName);
        if (string.IsNullOrWhiteSpace(node.Description)) return;
        builder.AppendLine(node.Description.Trim());
    }

    private static void ShowUsage(StringBuilder builder, IHasChildren node) {
        if (builder.Length != 0) builder.AppendLine();
        builder.AppendLine("Usage:");
        ShowDefaultUsage(builder, node);
        ShowUsageWithParameters(builder, node);
    }

    private static void ShowDefaultUsage(StringBuilder builder, IHasChildren node) {
        if (node.Commands.Length == 0 && node.Parameters.Length != 0) return;
        builder.Append(' ', _indentSize).Append(node.Path);
        if (node.Options.Length != 0) builder.Append(" [Options]");
        if (node.Commands.Length != 0) builder.Append(" [Commands]");
        builder.AppendLine();
    }

    private static void ShowUsageWithParameters(StringBuilder builder, IHasChildren node) {
        if (node.Parameters.Length == 0) return;
        builder.Append(' ', _indentSize).Append(node.Path);
        if (node.Options.Length != 0) builder.Append(" [Options]");
        foreach (var parameter in node.Parameters) {
            if (parameter.IsRequired) builder.Append($" <{parameter.Name}>");
            else builder.Append($" [<{parameter.Name}>]");
        }
        builder.AppendLine();
    }

    private static void ShowAliases(StringBuilder builder, IHasChildren node) {
        if (node.Aliases.Length == 0) return;
        builder.AppendLine();
        builder.Append("Aliases: ").AppendJoin(", ", node.Aliases).AppendLine();
    }

    private static void ShowItems(StringBuilder builder, string section, IReadOnlyCollection<INode> items) {
        if (items.Count == 0) return;
        builder.AppendLine();
        builder.AppendLine($"{section}:");
        foreach (var item in items)
            ShowItem(builder, item);
    }

    private static void ShowException(StringBuilder builder, Exception ex, bool isInner = false, byte indent = 0) {
        while (true) {
            ShowExceptionDescription(builder, ex, isInner, indent);
            ShowStackTrace(builder, ex, (byte)(indent + 1));
            if (ex.InnerException is null) break;
            ex = ex.InnerException;
            isInner = true;
            indent = (byte)(indent + 1);
        }
    }

    private static void ShowExceptionDescription(StringBuilder builder, Exception ex, bool isInner, int indent) {
        builder.Append(' ', indent * _indentSize);
        if (isInner) builder.Append("Inner Exception => ");
        builder.Append(ex.GetType().Name);
        builder.Append(": ");
        builder.AppendLine(ex.Message);
    }

    private static void ShowStackTrace(StringBuilder builder, Exception ex, byte indent) {
        if (string.IsNullOrEmpty(ex.StackTrace)) return;
        builder.Append(' ', indent * _indentSize).AppendLine("Stack Trace:");
        var lines = ex.StackTrace.Split(System.Environment.NewLine);
        _ = lines.Aggregate(builder, (s, l) => s.Append(' ', (indent + 1) * _indentSize).AppendLine(l));
    }

    public static void ShowItem(StringBuilder builder, INode node) {
        var itemId = new StringBuilder();
        itemId.Append(' ', _indentSize);
        ShowIds();
        var length = itemId.Length;
        builder.Append(itemId);
        ShowItemDescription();
        return;

        void ShowIds() {
            string[] ids = node is IArgument _
                               ? [$"--{node.Name.ToLower()}", .. node.Aliases.Select(a => $"-{a}")]
                               : [node.Name, .. node.Aliases];
            itemId.AppendJoin(", ", ids);
        }

        void ShowItemDescription() {
            var lines = node.Description.Split(System.Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) {
                builder.AppendLine();
                return;
            }
            builder.Append(' ', 30 - length).AppendLine(lines[0]);
            foreach (var line in lines.Skip(1)) builder.Append(' ', 30).AppendLine(line);
        }
    }
}
