namespace DotNetToolbox.Graph.Nodes;

public interface INode {
    string Id { get; }
    Result Validate(ICollection<INode>? validatedNodes = null);
    INode? Run(Context context);
}
