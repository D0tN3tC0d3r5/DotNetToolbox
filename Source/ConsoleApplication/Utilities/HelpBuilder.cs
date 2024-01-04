namespace DotNetToolbox.ConsoleApplication.Utilities;

internal class HelpBuilder {
    private readonly IHasChildren _parent;
    private readonly IReadOnlyCollection<INode> _options;
    private readonly IReadOnlyCollection<IParameter> _parameters;
    private readonly IReadOnlyCollection<ICommand> _commands;
    private readonly string _path;

    public HelpBuilder(IHasParent node, bool includeApplicationName) {
        _parent = node.Parent;
        _path = _parent.GetPath(includeApplicationName);
        _options = _parent.Children.OfType<IFlag>().Cast<INode>()
                          .Union(_parent.Children.OfType<IOption>())
                          .Union(_parent.Children.OfType<IAction>())
                          .OrderBy(i => i.Name).ToArray();
        _parameters = _parent.Children.OfType<IParameter>().OrderBy(i => i.Name).ToArray();
        _commands = _parent.Children.OfType<ICommand>().OrderBy(i => i.Name).ToArray();
    }

    public string Build() {
        var builder = new StringBuilder();
        ShowDescription(builder);
        ShowUsage(builder);
        ShowOptions(builder);
        ShowParameters(builder);
        ShowCommands(builder);
        return builder.ToString();
    }

    protected void ShowDescription(StringBuilder builder) {
        if (_parent is IApplication app) {
            app.AppendHelp(builder);
            return;
        }

        if (string.IsNullOrWhiteSpace(_parent.Description)) return;
        builder.AppendLine(_parent.Description);
    }

    protected void ShowUsage(StringBuilder builder) {
        if (string.IsNullOrEmpty(_path)) return;
        builder.AppendLine();
        builder.Append($"Usage: {_path}");
        if (_options.Count != 0) builder.Append(" [Options]");
        if (_parameters.Count != 0) builder.Append(" [Parameters]");
        if (_commands.Count != 0) builder.Append(" [Commands]");
        builder.AppendLine();
    }

    protected void ShowOptions(StringBuilder builder) {
        if (_options.Count <= 0) return;
        builder.AppendLine();
        builder.AppendLine("Options:");
        foreach (var option in _options)
            option.AppendHelp(builder);
    }

    protected void ShowParameters(StringBuilder builder) {
        if (_parameters.Count <= 0) return;
        builder.AppendLine();
        builder.AppendLine("Parameters:");
        foreach (var parameter in _parameters)
            parameter.AppendHelp(builder);
    }

    protected void ShowCommands(StringBuilder builder) {
        if (_commands.Count <= 0) return;
        builder.AppendLine();
        builder.AppendLine("Commands:");
        foreach (var command in _commands)
            command.AppendHelp(builder);
    }
}
