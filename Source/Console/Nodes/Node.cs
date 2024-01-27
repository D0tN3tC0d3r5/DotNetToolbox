using DotNetToolbox.ConsoleApplication.Application;

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
        Aliases = AllAreValid<string[], string>(aliases, IsValidAlias);
    }

    public IApplication Application { get; }
    public IHasChildren Parent { get; }
    public string Name { get; }
    public string[] Aliases { get; }
    public string Description { get; init; } = string.Empty;

    public abstract Task<Result> ExecuteAsync(IReadOnlyList<string> args, CancellationToken ct = default);

    private static IApplication FindRoot(INode node) {
        while (node is IHasParent hasParent) node = hasParent.Parent;
        return (IApplication)node;
    }

    private static bool IsValidName(string? name)
        => name is not null
        && name.Length > 2
        && char.IsLetter(name[0])
        && name[1..].All(c => char.IsLetterOrDigit(c) || "-_".Contains(c));

    private static bool IsValidAlias(string? alias)
        => !string.IsNullOrEmpty(alias)
        && alias.All(c => char.IsLetterOrDigit(c) || "!?@#$%&".Contains(c));

    protected ILogger<TNode> Logger { get; }

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendJoin(null, GetType().Name, ": ");
        builder.AppendJoin(", ", [Name, .. Aliases]);
        builder.AppendJoin(null, " => ", Description);
        return builder.ToString();
    }

    public void AppendHelp(StringBuilder builder) {
        var line = new StringBuilder();
        line.Append("  ");
        AppendIds();
        var length = line.Length;
        builder.Append(line);
        builder.Append(' ', 28 - length);
        AppendDescription();
        return;

        void AppendIds() {
            string[] ids = this is IOption _
                ? [$"--{Name.ToLower()}", .. Aliases.Select(a => $"-{a}")]
                : [Name, .. Aliases];
            line.AppendJoin(", ", ids);
        }

        void AppendDescription() {
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
