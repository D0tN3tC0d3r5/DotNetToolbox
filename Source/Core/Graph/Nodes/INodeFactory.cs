namespace DotNetToolbox.Graph.Nodes;

public interface INodeFactory {
    INode CreateEnd(string id);
}