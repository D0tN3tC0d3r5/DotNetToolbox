namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface INode {
    IExecutableNode? Parent { get; }
    ICollection<INamedNode> Children { get; }
}
