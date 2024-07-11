namespace DotNetToolbox.Graph;

public interface INodeFactory {
    INode CreateEnd(string id);
}