namespace AI.Sample.Main.Commands;

public class HelpCommand(IHasChildren parent)
    : Command<HelpCommand>(parent, "Help", n => {
        n.Aliases = ["?"];
        n.Description = "Show help";
        n.Help = "Display this help information.";
        n.AddParameter("Target", string.Empty);
    }) {
    private const int _indentSize = 4;

    protected override Result Execute() => this.HandleCommand(() => {
        Logger.LogInformation("Executing Help command...");
        ShowHelp();
        return Result.Success();
    }, "Error displaying the help information.");

    private void ShowHelp() {
        var target = Map.GetValueAs<string>("Target");
        var command = Parent.Commands.FirstOrDefault(i => i.Name.Equals(target, StringComparison.OrdinalIgnoreCase));
        var node = command ?? Parent;
        var helpText = GetHelp(node);
        Output.Write(helpText);
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
        if (string.IsNullOrWhiteSpace(node.Help)) return;
        builder.AppendLine(node.Help.Trim());
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
        var lines = node.Help.Split(System.Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0) {
            builder.AppendLine();
            return;
        }
        builder.Append(' ', 30 - length).AppendLine(lines[0]);
        foreach (var line in lines.Skip(1)) builder.Append(' ', 30).AppendLine(line);
    }
}
