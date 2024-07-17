namespace DotNetToolbox.Graph.Nodes;

public interface ISelectNode
    : ISelectNode<string>;

public interface ISelectNode<in TKey>
    : INode
    where TKey : notnull {
    void SetOption(TKey key, INode? path);
}
