namespace DotNetToolbox.Graph.Nodes;

public interface IMainBuilder
    : INodeBuilder {
    IfBuilder If(Func<Context, bool> predicate, IGuidProvider? guid = null);
}

public interface INodeBuilder {
    INode Build();
}

public abstract class NodeBuilder
    : INodeBuilder {

    protected NodeBuilder(Stack<INode>? stack = null, INode? caller = null, INodeFactory? factory = null) {
        NodeStack = stack ?? [];
        CallerNode = caller;
        Factory = factory ?? new NodeFactory();
    }

    protected INodeFactory Factory { get; }
    protected Stack<INode> NodeStack { get; }
    protected INode? CallerNode { get; }
    protected INode? CurrentNode { get; set; }

    public INode Build() {
        if (CurrentNode is null)
            return Node.Void;

        while (NodeStack.Count > 0) {
            var node = NodeStack.Pop();
            if (node is IIfNode ifNode) {
                ifNode.TruePath = ifNode.TruePath ?? Node.Void;
                ifNode.FalsePath = ifNode.FalsePath ?? Node.Void;
                ifNode.TruePath = IsNotNull(ifNode.TruePath);
                ifNode.FalsePath = IsNotNull(ifNode.FalsePath);
                NodeStack.Push(ifNode.TruePath);
                NodeStack.Push(ifNode.FalsePath);
            }
            else if (node is ISelectNode selectNode) {
                selectNode.Paths = selectNode.Paths.Select(IsNotNull).ToArray();
                foreach (var path in selectNode.Paths)
                    NodeStack.Push(path);
            }
        }

        return CurrentNode;
    }
}

public class MainBuilder(Stack<INode>? stack = null, INode? caller = null)
    : NodeBuilder(stack, caller) {
    public IfBuilder If(Func<Context, bool> predicate, IGuidProvider? guid = null) {
        CurrentNode = Factory.If(predicate, Node.Void, Node.Void, guid);
        NodeStack.Push(CurrentNode);
        return new(NodeStack, CurrentNode);
    }
}

public class IfBuilder(Stack<INode>? stack = null, INode? caller = null)
    : NodeBuilder(stack, caller) {
    public ThenBuilder Then(Action<MainBuilder> nodeBuilder) {
        if (CallerNode is not IfNode ifNode) {
            throw new InvalidOperationException("Then can only be called after an If");
        }
        var path = new MainBuilder();
        nodeBuilder(path);
        var falseNode = path.Build();
        ifNode.TruePath = IsNotNull(falseNode);
        return new(NodeStack, NodeStack.Pop());
    }
}

public class ThenBuilder(Stack<INode>? stack = null, INode? caller = null)
    : MainBuilder(stack, caller) {
    public MainBuilder Else(Action<MainBuilder> nodeBuilder) {
        if (CallerNode is not IfNode ifNode) {
            throw new InvalidOperationException("Then can only be called after an If");
        }
        var path = new MainBuilder();
        nodeBuilder(path);
        var falseNode = path.Build();
        ifNode.FalsePath = IsNotNull(falseNode);
        return new(NodeStack, NodeStack.Pop());
    }
}

public abstract class Node
    : INode {
    protected Node(string? id = null, IGuidProvider? guid = null) {
        guid ??= new GuidProvider();
        Id = id ?? guid.AsSortable.Create().ToString();
    }

    #region Factory

    private static readonly INodeFactory _factory = new NodeFactory();

    public static IIfNode If(Func<Context, bool> predicate, INode truePath, INode? falsePath = null, IGuidProvider? guid = null)
        => _factory.If(predicate, truePath, falsePath, guid);

    public static IIfNode If(string id, Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => _factory.If(id, predicate, truePath, falsePath);

    public static ISelectNode<TKey> Select<TKey>(Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths, IGuidProvider? guid = null)
        where TKey : notnull
        => _factory.Select(select, paths, guid);

    public static ISelectNode<TKey> Select<TKey>(string id, Func<Context, TKey> select, IReadOnlyDictionary<TKey, INode?> paths)
        where TKey : notnull
        => _factory.Select(id, select, paths);

    public static ISelectNode Select(Func<Context, string> select, IEnumerable<INode?> paths, IGuidProvider? guid = null)
        => _factory.Select(select, paths, guid);

    public static ISelectNode Select(string id, Func<Context, string> select, IEnumerable<INode?> paths)
        => _factory.Select(id, select, paths);

    public static IActionNode Do(Action<Context> action, INode? next = null, IGuidProvider? guid = null)
        => _factory.Do(action, next, guid);

    public static IActionNode Do(string id, Action<Context> action, INode? next = null)
        => _factory.Do(id, action, next);

    public static INode Void
        => _factory.Void;

    #endregion

    public string Id { get; }
    protected List<INode?> Paths { get; init; } = [];

    public virtual Result Validate(ICollection<INode>? validatedNodes = null) {
        validatedNodes ??= new HashSet<INode>();
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
    protected abstract void UpdateState(Context state);
    protected abstract INode? GetNext(Context state);
    protected virtual void OnExit(Context state) { }

    public override int GetHashCode() => Id.GetHashCode();
}
