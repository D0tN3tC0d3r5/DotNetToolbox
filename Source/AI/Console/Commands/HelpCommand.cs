namespace AI.Sample.Commands;

public class HelpCommand : Command<HelpCommand> {
    private const int _indentSize = 4;
    private readonly IHasChildren _parent;

    public HelpCommand(IHasChildren parent)
        : base(parent, "Help", ["?"]) {
        _parent = IsNotNull(parent);
        Description = "Display this help information.";
        AddParameter("Target", string.Empty);
    }

    public override Task<Result> Execute(CancellationToken ct = default) {
        var target = (string?)Context.GetValueOrDefault("Target");
        var command = _parent.Commands.FirstOrDefault(i => i.Name.Equals(target, StringComparison.OrdinalIgnoreCase));
        var node = command ?? _parent;
        var helpText = GetHelp(node);
        AnsiConsole.Markup(helpText);
        return Result.SuccessTask();
    }

    private static string GetHelp(IHasChildren node) {
        var builder = new StringBuilder();
        AppendNodeDescription(builder, node);
        AppendUsage(builder, node);
        AppendAliases(builder, node);
        AppendItems(builder, "Options", node.Options);
        AppendItems(builder, "Parameters", node.Parameters);
        AppendItems(builder, "Commands", node.Commands);
        return builder.ToString();
    }

    private static void AppendNodeDescription(StringBuilder builder, INode node) {
        if (node is IApplication app) builder.AppendLine(app.FullName);
        if (string.IsNullOrWhiteSpace(node.Description)) return;
        builder.AppendLine(node.Description.Trim());
    }

    private static void AppendUsage(StringBuilder builder, IHasChildren node) {
        if (builder.Length != 0) builder.AppendLine();
        builder.AppendLine("Usage:");
        AppendDefaultUsage(builder, node);
        AppendUsageWithParameters(builder, node);
    }

    private static void AppendDefaultUsage(StringBuilder builder, IHasChildren node) {
        if (node.Commands.Length == 0 && node.Parameters.Length != 0) return;
        builder.Append(' ', _indentSize).Append(node.Path);
        if (node.Options.Length != 0) builder.Append(" [[Options]]");
        if (node.Commands.Length != 0) builder.Append(" [[Commands]]");
        builder.AppendLine();
    }

    private static void AppendUsageWithParameters(StringBuilder builder, IHasChildren node) {
        if (node.Parameters.Length == 0) return;
        builder.Append(' ', _indentSize).Append(node.Path);
        if (node.Options.Length != 0) builder.Append(" [[Options]]");
        foreach (var parameter in node.Parameters) {
            if (parameter.IsRequired) builder.Append($" <{parameter.Name}>");
            else builder.Append($" [<{parameter.Name}>]");
        }
        builder.AppendLine();
    }

    private static void AppendAliases(StringBuilder builder, IHasChildren node) {
        if (node.Aliases.Length == 0) return;
        builder.AppendLine();
        builder.Append("Aliases: ").AppendJoin(", ", node.Aliases).AppendLine();
    }

    private static void AppendItems(StringBuilder builder, string section, IReadOnlyCollection<INode> items) {
        if (items.Count == 0) return;
        builder.AppendLine();
        builder.AppendLine($"{section}:");
        foreach (var item in items)
            AppendItem(builder, item);
    }

    private static void AppendItem(StringBuilder builder, INode node) {
        builder.Append(' ', _indentSize);
        var ids = GetIds(node);
        builder.Append(ids);
        AppendNodeDescription(builder, node, ids.Length + _indentSize);
    }

    private static string GetIds(INode node) {
        string[] ids = node is IArgument _
                           ? [$"--{node.Name.ToLowerInvariant()}", .. node.Aliases.Select(a => $"-{a}")]
                           : [node.Name, .. node.Aliases];
        return string.Join(", ", ids);
    }

    private static void AppendNodeDescription(StringBuilder builder, INode node, int length) {
        var lines = node.Description.Split(System.Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0) {
            builder.AppendLine();
            return;
        }
        builder.Append(' ', 30 - length).AppendLine(lines[0]);
        foreach (var line in lines.Skip(1)) builder.Append(' ', 30).AppendLine(line);
    }

    //    private static void AppendException(StringBuilder builder, Exception ex, bool isInner = false, byte indent = 0) {
    //        while (true) {
    //            AppendExceptionDescription(builder, ex, isInner, indent);
    //            ShowStackTrace(builder, ex, (byte)(indent + 1));
    //            if (ex.InnerException is null) break;
    //            ex = ex.InnerException;
    //            isInner = true;
    //            indent = (byte)(indent + 1);
    //        }
    //    }

    //    private static void AppendExceptionDescription(StringBuilder builder, Exception ex, bool isInner, int indent) {
    //        builder.Append(' ', indent * _indentSize);
    //        if (isInner) builder.Append("Inner Exception => ");
    //        builder.Append(ex.GetType().Name);
    //        builder.Append(": ");
    //        builder.AppendLine(ex.Message);
    //    }

    //    private static void ShowStackTrace(StringBuilder builder, Exception ex, byte indent) {
    //        if (string.IsNullOrEmpty(ex.StackTrace)) return;
    //        builder.Append(' ', indent * _indentSize).AppendLine("Stack Trace:");
    //        var lines = ex.StackTrace.Split(System.Environment.NewLine);
    //#pragma warning disable CA1806 // Do not ignore method results - contains side effects
    //        lines.Aggregate(builder, (s, l) => s.Append(' ', (indent + 1) * _indentSize).AppendLine(l));
    //#pragma warning restore CA1806 // Do not ignore method results
    //    }
}
