namespace DotNetToolbox.Graph.Nodes;

public class ActionNode
    : Node {
    private readonly Action<Context> _execute;
    private INode? _exit;
    protected INode ExitNode {
        get => _exit ?? throw new InvalidOperationException($"The exit node for node {Id} is not set.");
        set => SetExit(value);
    }

    public ActionNode(string id, INode? exitNode = null, Action<Context>? execute = null)
        : base(id) {
        _execute = execute ?? Execute;
        ExitNode = exitNode ?? ExitNode;
    }

    private void SetExit(INode node) {
        _exit = IsNotNull(node);
        Paths.Add(node);
    }

    protected override Result IsValid() {
        var result = Success();
        if (Paths.Count != 0)
            result += Invalid($"Void node '{Id}' can not have any exits.");
        return result;
    }

    protected sealed override INode GetNext(Context state) => ExitNode;
    protected sealed override void UpdateState(Context state) => _execute(state);

    protected virtual void Execute(Context state)
        => throw new NotImplementedException($"The update action is not defined for node '{Id}'.");
}
