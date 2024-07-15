
namespace DotNetToolbox.Graph.Nodes;

public sealed class VoidNode()
    : Node("{VOID}", null) {
    public override Result Validate(ICollection<INode> validatedNodes)
        => Success();

    public override INode? Run(Context context) => null;
}
