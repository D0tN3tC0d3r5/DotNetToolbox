namespace DotNetToolbox.ConsoleApplication.Nodes;

public interface INamedNode : INode {
    string[] Ids { get; }
    string Name { get; }
    string? Alias { get; }
    string? Description { get; }
}
