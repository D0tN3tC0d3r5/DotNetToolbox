namespace DotNetToolbox.Graph.Nodes;

public abstract class Node
    : INode {
    protected Node(string? id, IGuidProvider? guid) {
        guid ??= new GuidProvider();
        Id = id ?? guid.Create().ToString();
    }

    #region Factory

    private static readonly INodeFactory _factory = new NodeFactory();

    public static INode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => _factory.If(predicate, truePath, falsePath);

    public static INode Select<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull
        => _factory.Select(select, paths);

    public static INode Select(Func<Context, string> select, IEnumerable<INode?> paths)
        => _factory.Select(select, paths);

    public static INode Do(Action<Context> action, INode? next = null)
        => _factory.Do(action, next);

    public static INode Void
        => _factory.Void;

    #endregion

    public string Id { get; }
    protected HashSet<INode?> Paths { get; init; } = [];

    public virtual Result Validate(ICollection<INode> validatedNodes) {
        if (validatedNodes.Contains(this))
            return Success();

        var result = IsValid();
        if (result.IsInvalid)
            return result;

        validatedNodes.Add(this);
        return Paths.Where(node => node is not null)
                    .Aggregate(result, (current, node) => current + node!.Validate(validatedNodes));
    }

    protected virtual Result IsValid() => Success();

    public virtual INode? Run(Context context) {
        OnEntry(context);
        UpdateState(context);
        var exitPath = GetNext(context);
        OnExit(context);
        return exitPath;
    }

    protected virtual void OnEntry(Context state) { }
    protected virtual void UpdateState(Context state) { }
    protected virtual INode? GetNext(Context state) => null;
    protected virtual void OnExit(Context state) { }

    public override int GetHashCode() => Id.GetHashCode();
}
