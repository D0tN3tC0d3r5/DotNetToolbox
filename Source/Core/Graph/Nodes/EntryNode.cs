namespace DotNetToolbox.Graph.Nodes;

public sealed class EntryNode
    : Node,
      IEntryNode {
    private EntryNode(INode? next = null,
                      INodeFactory? factory = null)
        : base(factory) {
        Next = next;
    }

    public static EntryNode Create(INodeFactory? factory = null)
        => new(next: null, factory);

    public INode? Next {
        get => Branches[0];
        set => Branches[0] = value;
    }

    protected override INode? GetNext(Context context)
        => Next;

    protected override void UpdateState(Context context) { }
}
