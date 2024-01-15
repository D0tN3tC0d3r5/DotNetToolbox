namespace DotNetToolbox.ConsoleApplication.Nodes;

public abstract class Node<TNode>
    : IHasParent
    where TNode : Node<TNode> {
    protected Node(IHasChildren parent, string name, params string[] aliases) {
        Parent = parent;
        Application = FindRoot(this);
        Logger = Application.ServiceProvider.GetRequiredService<ILogger<TNode>>();
        Name = IsValid(name, n => !string.IsNullOrWhiteSpace(n)
                               && !n.StartsWith('-')
                               && n.All(c => char.IsLetterOrDigit(c) || c == '-'))!;
        Aliases = AllAreValid<string[], string>(aliases, n => !string.IsNullOrWhiteSpace(n)
                                         && !n.StartsWith('-')
                                         && n.All(c => char.IsLetterOrDigit(c) || c == '-'))!;
    }
    private static IApplication FindRoot(INode node) {
        while (node is IHasParent hasParent) node = hasParent.Parent;
        return (IApplication)node;
    }

    protected ILogger<TNode> Logger { get; }
    public IApplication Application { get; }
    public IHasChildren Parent { get; }
    public string Name { get; }
    public string[] Aliases { get; }
    public string[] Ids => [Name, .. Aliases];
    public string Description { get; init; } = string.Empty;

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendJoin(null, GetType().Name, ": ");
        builder.AppendJoin(", ", Ids);
        builder.AppendJoin(null, " => ", Description);
        return builder.ToString();
    }

    public void AppendHelp(StringBuilder builder) {
        var commands = new StringBuilder();
        commands.Append("  ");
        commands.AppendJoin(" | ", Ids);
        commands.Append(' ', 28 - commands.Length);
        builder.Append(commands);
        AppendWrappedDescription();
        return;

        void AppendWrappedDescription() {
            var lines = Description.Split(Environment.NewLine);
            var chunks = lines.SelectMany(i => i.Chunk(40)).Select(i => string.Join(string.Empty, i)).ToArray();
            builder.AppendLine(chunks.FirstOrDefault() ?? string.Empty);
            if (chunks.Length == 1) return;
            foreach (var chunk in chunks.Skip(1)) {
                builder.Append(' ', 30);
                builder.AppendLine(chunk);
            }
        }
    }
}
