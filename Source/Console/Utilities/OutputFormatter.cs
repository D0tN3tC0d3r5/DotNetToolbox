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

    public static string FormatHelp(IHasChildren parent, bool includeApplication) {
        var path = parent.GetPath(includeApplication);
        var options = parent
                     .Children.Where(i => i.Name.StartsWith('-'))
                     .OrderBy(i => i.Name).ToArray();
        var parameters = parent.Children.OfType<IParameter>().OrderBy(i => i.Name).ToArray();
        var commands = parent.Children.OfType<ICommand>().Where(c => !c.Name.StartsWith('-')).OrderBy(i => i.Name).ToArray();

        var builder = new StringBuilder();
        ShowDescription(builder, parent);
        builder.AppendLine();
        ShowUsage(builder, path, options, parameters, commands);
        ShowOptions(builder, options);
        ShowParameters(builder, parameters);
        ShowCommands(builder, commands);
        return builder.ToString();
    }

    private static void ShowDescription(StringBuilder builder, IHasChildren parent) {
        if (parent is IApplication app) builder.AppendLine(app.FullName);
        if (string.IsNullOrWhiteSpace(parent.Description)) return;
        builder.AppendLine(parent.Description);
        builder.AppendLine();
    }

    private static void ShowUsage(StringBuilder builder,
                                  string? path,
                                  IReadOnlyCollection<INode> options,
                                  IReadOnlyCollection<IParameter> parameters,
                                  IReadOnlyCollection<ICommand> commands) {
        if (string.IsNullOrEmpty(path)) return;
        builder.Append("Usage: ").Append(path);
        if (options.Count != 0) builder.Append(" [Options]");
        if (parameters.Count != 0) builder.Append(" [Parameters]");
        if (commands.Count != 0) builder.Append(" [Commands]");
        builder.AppendLine();
    }

    private static void ShowOptions(StringBuilder builder, IReadOnlyCollection<INode> options) {
        if (options.Count == 0) return;
        builder.AppendLine("Options:");
        foreach (var option in options)
            option.AppendHelp(builder);
        builder.AppendLine();
    }

    private static void ShowParameters(StringBuilder builder, IReadOnlyCollection<IParameter> parameters) {
        if (parameters.Count == 0) return;
        builder.AppendLine("Parameters:");
        foreach (var parameter in parameters)
            parameter.AppendHelp(builder);
        builder.AppendLine();
    }

    private static void ShowCommands(StringBuilder builder, ICommand[] commands) {
        if (commands.Length == 0) return;
        builder.AppendLine("Commands:");
        foreach (var command in commands)
            command.AppendHelp(builder);
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
