namespace DotNetToolbox.Graph.Nodes;

public class IfThenElseNode
    : Node {
    private readonly Func<Map, bool> _predicate;

    private INode? _trueExit;
    protected INode TruePath {
        get => _trueExit ?? throw new InvalidOperationException($"The true exit node for node {Id} is not set.");
        set => SetTruePath(value);
    }

    private INode? _falseExit;
    protected INode FalsePath {
        get => _falseExit ?? throw new InvalidOperationException($"The false exit node for node {Id} is not set.");
        set => SetFalsePath(value);
    }

    public IfThenElseNode(string id, INode? trueNode = null, INode? falseNode = null, Func<Map, bool>? predicate = null)
        : base(id) {
        _predicate = predicate ?? Predicate;
        TruePath = trueNode ?? TruePath;
        FalsePath = falseNode ?? FalsePath;
    }

    internal void SetTruePath(INode node) {
        IsNotNull(node);
        Exits.Remove(node);
        _trueExit = node;
        Exits.Add(node);
    }

    internal void SetFalsePath(INode node) {
        IsNotNull(node);
        Exits.Remove(node);
        _falseExit = node;
        Exits.Add(node);
    }

    protected override Result IsValid() {
        var result = Success();
        if (_trueExit is null)
            result += Invalid($"Missing true exit node for node '{Id}'.");
        if (_falseExit is null)
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
