using System.Diagnostics.CodeAnalysis;

namespace DotNetToolbox.Graph.Nodes;

public class IfNode
    : Node
    , IIfNode {
    private readonly Func<Context, bool> _predicate;

    public static IfNode Create(string id, Func<Context, bool> predicate, INode truePath, INode? falsePath = null)
        => new(IsNotNullOrWhiteSpace(id), predicate, truePath, falsePath);

    public static IfNode Create(Func<Context, bool> predicate, INode truePath, INode? falsePath = null, IGuidProvider? guid = null)
        => new(null, predicate, truePath, falsePath, guid);

    [SetsRequiredMembers]
    private IfNode(string? id, Func<Context, bool> predicate, INode trueNode, INode? falseNode, IGuidProvider? guid = null)
        : base(id, guid) {
        _predicate = IsNotNull(predicate);
        TruePath = trueNode;
        FalsePath = falseNode;
    }

    public INode? TruePath {
        get => Paths.Count > 0 ? Paths[0] : throw new InvalidOperationException("The true path is not defined.");
        set {
            if (Paths.Count == 0) Paths.Add(value);
            else Paths[0] = value;
        }
    }

    public INode? FalsePath {
        get => Paths.Count > 1 ? Paths[1] : Void;
        set {
            if (Paths.Count == 1) Paths.Add(value);
            else Paths[1] = value;
        }
    }

    protected sealed override INode? GetNext(Context state)
        => _predicate(state)
            ? TruePath
            : FalsePath;

    protected sealed override void UpdateState(Context state) { }
}
