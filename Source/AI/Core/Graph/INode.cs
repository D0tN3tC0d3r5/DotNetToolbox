namespace DotNetToolbox.AI.Graph;

public interface INode {
    string Id { get; }
    HashSet<INode?> Entries { get; }
    HashSet<INode?> Exits { get; }
    INode? Execute(Map state, INode? caller = null);
}
