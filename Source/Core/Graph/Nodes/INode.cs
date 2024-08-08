namespace DotNetToolbox.Graph.Nodes;

public interface INode {
    uint Id { get; }
    string Label { get; }
    Result Validate(ISet<INode>? visited = null);
    Task<INode?> Run(Context context, CancellationToken ct = default);
}
