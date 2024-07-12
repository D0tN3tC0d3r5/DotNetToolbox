namespace DotNetToolbox.Graph.Nodes;

public class IfNode
    : Node {
    private readonly Func<Map, bool> _predicate;

    public static IfNode Create(Func<Map, bool> predicate)
        => new(Guid.NewGuid().ToString(), IsNotNull(predicate));

    public IfNode(string id, Func<Map, bool>? predicate = null)
        : base(id) {
        _predicate = predicate ?? Predicate;
    }

    private INode? _truePath;
    protected INode TruePath {
        get => _truePath ?? throw new InvalidOperationException($"The true exit node for node {Id} is not set.");
        set => SetTruePath(value);
    }

    private INode? _falsePath;
    protected INode FalsePath {
        get => _falsePath ?? throw new InvalidOperationException($"The false exit node for node {Id} is not set.");
        set => SetFalsePath(value);
    }

    internal void SetTruePath(INode node) {
        IsNotNull(node);
        Exits.Remove(node);
        _truePath = node;
        Exits.Add(node);
    }

    internal void SetFalsePath(INode node) {
        IsNotNull(node);
        Exits.Remove(node);
        _falsePath = node;
        Exits.Add(node);
    }

    protected override Result IsValid() {
        var result = Success();
        if (_truePath is null)
            result += Invalid($"Missing true exit node for node '{Id}'.");
        if (_falsePath is null)
            result += Invalid($"Missing false exit node for node '{Id}'.");
        return result;
    }

    protected sealed override INode GetNext(Map state)
        => _predicate(state)
            ? TruePath
            : FalsePath;
    protected sealed override void UpdateState(Map state) => base.UpdateState(state);

    protected virtual bool Predicate(Map state)
        => throw new NotImplementedException($"The predicate is not defined for node '{Id}'.");
}
