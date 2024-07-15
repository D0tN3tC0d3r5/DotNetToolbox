
namespace DotNetToolbox.Graph.Nodes;

public sealed class VoidNode()
    : Node(Guid.Empty.ToString()) {
    public override Result Validate(ICollection<INode> validatedNodes)
        => Success();

    public override INode? Run(Context context) => null;
}
