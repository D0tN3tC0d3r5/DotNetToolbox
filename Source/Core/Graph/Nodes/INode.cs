namespace DotNetToolbox.Graph.Nodes;

public interface INode {
    string Id { get; }
    Map State { get; set; }
    Result Validate(ICollection<INode> validatedNodes);
    INode? Run(Map state);
}
