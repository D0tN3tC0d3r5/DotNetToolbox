namespace DotNetToolbox.Graph.Nodes;

public interface IMapNode
    : IMapNode<string>;

public interface IMapNode<in TKey>
    : INode
    where TKey : notnull {
    void SetOption(TKey key, INode? path);
}
