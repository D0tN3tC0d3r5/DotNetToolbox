namespace DotNetToolbox.Graph.Nodes;

public class ActionNode
    : Node {
    private readonly Action<Context> _execute;
    private readonly INode? _next;
    public static ActionNode Create(string id, Action<Context> execute, INode? next = null)
        => new(IsNotNullOrWhiteSpace(id), execute, next);

    public static ActionNode Create(Action<Context> execute, INode? next = null, IGuidProvider? guid = null)
        => new(null, execute, next, guid);

    private ActionNode(string? id, Action<Context> execute, INode? next = null, IGuidProvider? guid = null)
        : base(id, guid) {
        _execute = execute ?? Execute;
        _next = next ?? Void;
    }

    protected sealed override INode? GetNext(Context state) => _next;
    protected sealed override void UpdateState(Context state) => _execute(state);

    protected virtual void Execute(Context state)
        => throw new NotImplementedException($"The update action is not defined for node '{Id}'.");
}
