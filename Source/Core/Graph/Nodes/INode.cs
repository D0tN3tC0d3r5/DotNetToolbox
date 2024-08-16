namespace DotNetToolbox.Graph.Nodes;

public interface INode {
    uint Id { get; }
    string Tag { get; }
    string Label { get; }

    Token? Token { get; set; }
    INode? Next { get; set; }

    Result ConnectTo(INode? next);
    Result Validate(ISet<INode>? visited = null);
    Task<INode?> Run(Context context, CancellationToken ct = default);
}
