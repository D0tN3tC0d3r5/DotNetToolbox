namespace DotNetToolbox.Graph.Nodes;

public sealed class VoidNode()
    : Node(_id) {

    private const string _id = "{VOID}";
    public override Result Validate(ICollection<INode>? validatedNodes = null)
        => Success();

    public override INode? Run(Context context) => null;
    protected override void UpdateState(Context context) { }

    protected override INode? GetNext(Context context) => null;
}
