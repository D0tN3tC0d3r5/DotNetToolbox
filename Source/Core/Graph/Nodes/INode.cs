namespace DotNetToolbox.Graph.Nodes;

public interface INode {
    string Id { get; }
    Result Validate(ICollection<INode> validatedNodes);
    INode? Run(Context context);
}
