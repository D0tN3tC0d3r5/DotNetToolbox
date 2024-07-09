namespace DotNetToolbox.AI.Graph;

public interface INode {
    uint Id { get; }
    INode? Caller { get; }
    HashSet<INode?>? Branches { get; }
    INode? Execute(Map state);
}
