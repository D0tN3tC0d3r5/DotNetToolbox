namespace DotNetToolbox.ConsoleApplication.Nodes;

public abstract class Node<TNode>
    : IHasParent
    where TNode : Node<TNode> {
    protected Node(IHasChildren parent, string name, params string[] aliases) {
        Parent = parent;
        Application = FindRoot(this);
        var factory = Application.Services.GetRequiredService<ILoggerFactory>();
        Logger = factory.CreateLogger<TNode>();
        Name = IsNotNull(name);
        Name = IsValid(name, IsValidName);
        Aliases = AllAreValid<string[], string>(aliases, IsValidName);
    }

    public IApplication Application { get; }
    public IHasChildren Parent { get; }
    public string Name { get; }
    public string[] Aliases { get; }
    public string[] Ids => [Name, .. Aliases];
    public string Description { get; init; } = string.Empty;

    public Task<Result> ExecuteAsync(CancellationToken ct = default) => ExecuteAsync(Array.Empty<string>(), ct);

    public abstract Task<Result> ExecuteAsync(IReadOnlyList<string> args, CancellationToken ct = default);

    private static IApplication FindRoot(INode node) {
        while (node is IHasParent hasParent) node = hasParent.Parent;
        return (IApplication)node;
    }

    private static bool IsValidName(string? name)
        => !string.IsNullOrWhiteSpace(name)
        && name.Length > 1
        && (char.IsLetter(name[0]) || name[0] == '-')
        && name[1..].All(c => char.IsLetterOrDigit(c) || "-!@#$%?&:=".Contains(c));

    protected ILogger<TNode> Logger { get; }

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendJoin(null, GetType().Name, ": ");
        builder.AppendJoin(", ", Ids);
        builder.AppendJoin(null, " => ", Description);
        return builder.ToString();
    }

    public void AppendHelp(StringBuilder builder) {
        var command = new StringBuilder();
        command.Append("  ");
        command.AppendJoin(", ", Ids);
        command.Append(' ', 28 - command.Length);
        builder.Append(command);
        AppendWrappedDescription();
        return;

        void AppendWrappedDescription() {
            var lines = Description.Split(Environment.NewLine);
            builder.AppendLine(lines.FirstOrDefault() ?? string.Empty);
            if (lines.Length == 1) return;
            foreach (var line in lines.Skip(1)) {
                builder.Append(' ', 30);
                builder.AppendLine(line);
            }
        }
    }
}
