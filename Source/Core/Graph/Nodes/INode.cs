namespace DotNetToolbox.Graph.Nodes;

public interface INode {
    string Id { get; }
    INode? Next { get; set; }
    Result Validate(ICollection<INode>? validatedNodes = null);
    INode? Run(Context context);
}

public interface INode<TKey>
    : INode
    where TKey : notnull {
    Dictionary<TKey, INode?> Branches { get; }
}
