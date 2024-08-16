namespace DotNetToolbox.Graph.Nodes;

public interface INode {
    uint Id { get; }
    string Tag { get; }
    string Label { get; }
    INode? Next { get; set; }
    Result Validate(ISet<INode>? visited = null);
    Task<INode?> Run(Context context, CancellationToken ct = default);
    Result ConnectTo(INode? next, Token? token);
}
