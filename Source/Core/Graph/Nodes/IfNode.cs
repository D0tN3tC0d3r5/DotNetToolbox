namespace DotNetToolbox.Graph.Nodes;

public class IfNode
    : Node {
    private readonly Func<Context, bool> _predicate;
    private INode? _truePath;
    private INode? _falsePath;

    public static IfNode Create(Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => new(Guid.NewGuid().ToString(), predicate, truePath, falsePath);

    private IfNode(string id, Func<Context, bool> predicate, INode trueNode, INode? falseNode = null)
        : base(id) {
        _predicate = IsNotNull(predicate);
        _truePath = IsNotNull(trueNode);
        _falsePath = falseNode ?? NodeBuilder.Start.Void;
    }

    protected INode TruePath {
        get => _truePath ?? throw new InvalidOperationException($"The true exit node for node {Id} is not set.");
        set => SetTruePath(value);
    }

    protected INode? FalsePath {
        get => _falsePath;
        set => SetFalsePath(value);
    }

    private void SetTruePath(INode node) {
        IsNotNull(node);
        _truePath = node;
        Paths.Add(_truePath);
    }

    private void SetFalsePath(INode? node) {
        if (node is null)
            return;

        _falsePath = node;
        Paths.Add(_falsePath);
    }

    protected override Result IsValid() {
        var result = Success();
        if (_truePath is null)
            result += Invalid($"Missing true exit node for node '{Id}'.");
        if (_falsePath is null)
            result += Invalid($"Missing false exit node for node '{Id}'.");
        return result;
    }

    protected sealed override INode? GetNext(Context state)
        => _predicate(state)
            ? TruePath
            : FalsePath;

    protected sealed override void UpdateState(Context state) => base.UpdateState(state);
}
