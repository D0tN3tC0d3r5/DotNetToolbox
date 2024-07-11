namespace DotNetToolbox.Graph;

public interface INode {
    string Id { get; }
    Map State { get; set; }
    void ConnectTo(INode node, object? metadata = null);
    INode? Execute(INode caller);
}
