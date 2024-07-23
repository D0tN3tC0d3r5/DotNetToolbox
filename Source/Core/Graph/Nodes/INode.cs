namespace DotNetToolbox.Graph.Nodes;

public interface INode {
    uint Id { get; }
    string Label { get; }
    INode? Next { get; set; }
    Result Validate(ISet<INode>? visited = null);
    INode? Run(Context context);
}
