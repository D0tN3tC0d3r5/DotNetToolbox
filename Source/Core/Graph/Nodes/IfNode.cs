using System.Diagnostics.CodeAnalysis;

namespace DotNetToolbox.Graph.Nodes;

public class IfNode
    : Node {
    private readonly Func<Context, bool> _predicate;
    private readonly INode _truePath;
    private readonly INode _falsePath;

    public static IfNode Create(string id, Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => new(IsNotNullOrWhiteSpace(id), predicate, truePath, falsePath);

    public static IfNode Create(Func<Context, bool> predicate, INode truePath, INode? falsePath = null, IGuidProvider? guid = null)
        => new(null, predicate, truePath, falsePath, guid);

    [SetsRequiredMembers]
    private IfNode(string? id, Func<Context, bool> predicate, INode trueNode, INode? falseNode, IGuidProvider? guid = null)
        : base(id, guid) {
        _predicate = IsNotNull(predicate);
        _truePath = IsNotNull(trueNode);
        _falsePath = falseNode ?? Void;
        Paths.Add(_truePath);
        Paths.Add(_falsePath);
    }

    protected sealed override INode? GetNext(Context state)
        => _predicate(state)
            ? _truePath
            : _falsePath;

    protected sealed override void UpdateState(Context state)
        => base.UpdateState(state);
}
