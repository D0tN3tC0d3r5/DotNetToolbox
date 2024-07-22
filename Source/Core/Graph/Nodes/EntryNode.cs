namespace DotNetToolbox.Graph.Nodes;

public sealed class EntryNode
    : ActionNode<EntryNode> {
    private EntryNode(INode? next = null,
                      IGuidProvider? guid = null)
        : base(guid) {
        Next = next;
    }

    public static EntryNode Create(IGuidProvider? guid = null)
        => new(next: null, guid);

    public static EntryNode Create(INode next,
                                   IGuidProvider? guid = null)
        => new(IsNotNull(next), guid);

    protected override void Execute(Context context) { }
}
