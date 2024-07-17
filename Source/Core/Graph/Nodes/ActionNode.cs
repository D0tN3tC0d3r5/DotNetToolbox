namespace DotNetToolbox.Graph.Nodes;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Public API
public class ActionNode
    : Node
    , IActionNode {
    private readonly Action<Context> _execute;
    public static ActionNode Create(string id, Action<Context> execute, INode? next = null)
        => new(IsNotNullOrWhiteSpace(id), execute, next);

    public static ActionNode Create(Action<Context> execute, INode? next = null, IGuidProvider? guid = null)
        => new(null, execute, next, guid);

    private ActionNode(string? id, Action<Context> execute, INode? next = null, IGuidProvider? guid = null)
        : base(id, guid) {
        _execute = execute ?? Execute;
        Next = next;
    }

    public INode? Next {
        get => Paths.ElementAtOrDefault(0);
        set {
            if (Paths.Count == 0) Paths.Add(value);
            else Paths[0] = value;
        }
    }

    protected sealed override INode? GetNext(Context state) => Next;
    protected sealed override void UpdateState(Context state) => _execute(state);

    protected virtual void Execute(Context state)
        => throw new NotImplementedException($"The update action is not defined for node '{Id}'.");
}
