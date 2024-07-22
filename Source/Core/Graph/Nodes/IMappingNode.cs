namespace DotNetToolbox.Graph.Nodes;

public interface IMappingNode
    : IMappingNode<string>;

public interface IMappingNode<in TKey>
    : INode
    where TKey : notnull {
    void SetOption(TKey key, INode? path);
}
