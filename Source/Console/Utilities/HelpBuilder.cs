namespace DotNetToolbox.ConsoleApplication.Utilities;

internal class HelpBuilder(IHasChildren parent, bool includeApplication) {
    private readonly IReadOnlyCollection<INode> _options = parent
                          .Children.OfType<IFlag>().Cast<INode>()
                          .Union(parent.Children.OfType<IOption>())
                          .Union(parent.Children.OfType<IAction>())
                          .OrderBy(i => i.Name).ToArray();
    private readonly IReadOnlyCollection<IParameter> _parameters = parent.Children.OfType<IParameter>().OrderBy(i => i.Name).ToArray();
    private readonly IReadOnlyCollection<ICommand> _commands = parent.Children.OfType<ICommand>().OrderBy(i => i.Name).ToArray();
    private readonly string _path = parent.GetPath(includeApplication);

    public string Build() {
        var builder = new StringBuilder();
        builder.AppendLine();
        ShowDescription(builder);
        ShowUsage(builder);
        ShowOptions(builder);
        ShowParameters(builder);
        ShowCommands(builder);
        return builder.ToString();
    }

    protected void ShowDescription(StringBuilder builder) {
        if (parent is IApplication app) builder.AppendLine(app.FullName);
        if (string.IsNullOrWhiteSpace(parent.Description)) return;
        builder.AppendLine(parent.Description);
        builder.AppendLine();
    }

    protected void ShowUsage(StringBuilder builder) {
        if (string.IsNullOrEmpty(_path)) return;
        builder.Append($"Usage: {_path}");
        if (_options.Count != 0) builder.Append(" [Options]");
        if (_parameters.Count != 0) builder.Append(" [Parameters]");
        if (_commands.Count != 0) builder.Append(" [Commands]");
        builder.AppendLine();
    }

    protected void ShowOptions(StringBuilder builder) {
        if (_options.Count <= 0) return;
        builder.AppendLine("Options:");
        foreach (var option in _options)
            option.AppendHelp(builder);
        builder.AppendLine();
    }

    protected void ShowParameters(StringBuilder builder) {
        if (_parameters.Count <= 0) return;
        builder.AppendLine("Parameters:");
        foreach (var parameter in _parameters)
            parameter.AppendHelp(builder);
        builder.AppendLine();
    }

    protected void ShowCommands(StringBuilder builder) {
        if (_commands.Count <= 0) return;
        builder.AppendLine("Commands:");
        foreach (var command in _commands)
            command.AppendHelp(builder);
        builder.AppendLine();
    }
}
