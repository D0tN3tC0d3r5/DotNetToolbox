namespace DotNetToolbox.Graph.Nodes;

public interface INode {
    uint Id { get; }

    string Tag { get; set; }
    string Label { get; set; }
    Token? Token { get; set; }
    INode? Next { get; set; }

    void ConnectTo(INode? next);
    Result Validate(ISet<INode>? visited = null);
    Task<INode?> Run(Context context, CancellationToken ct = default);
}
