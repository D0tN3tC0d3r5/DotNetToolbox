namespace DotNetToolbox.ConsoleApplication.Nodes;

public abstract class Node<TNode>
    : IHasParent
    where TNode : Node<TNode> {

    protected Node(IHasChildren parent, string name, params string[] aliases) {
        Parent = parent;
        Application = FindRoot(this);
        Environment = Application.Environment;
        PromptFactory = new PromptFactory(Environment);
        var factory = Application.Services.GetRequiredService<ILoggerFactory>();
        Logger = factory.CreateLogger<TNode>();
        Name = IsValid(name, IsValidName);
        Aliases = DoesNotContainInvalidItems<string[], string>(aliases, IsValidAlias);
    }

    public IApplication Application { get; }
    public IEnvironment Environment { get; }
    public IPromptFactory PromptFactory { get; }
    public IHasChildren Parent { get; }
    public string Name { get; }
    public string[] Aliases { get; }
    public string Description { get; init; } = string.Empty;

    protected ILogger<TNode> Logger { get; }

    public override string ToString()
        => $"{GetType().Name}: {string.Join(", ", [Name, .. Aliases])} => {Description}";

    private static IApplication FindRoot(INode node) {
        while (node is IHasParent hasParent) node = hasParent.Parent;
        return (IApplication)node;
    }

    private static bool IsValidName(string? name)
        => name is not null
        && name.Length > 1
        && char.IsLetter(name[0])
        && name[1..].All(c => char.IsLetterOrDigit(c) || "-_".Contains(c));

    private static bool IsValidAlias(string? alias)
        => !string.IsNullOrEmpty(alias)
        && alias.All(c => char.IsLetterOrDigit(c) || "!?@#$%&".Contains(c));
}
