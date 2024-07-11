namespace DotNetToolbox.Graph;

public class NodeFactory : INodeFactory {
    public INode CreateEnd(string id)
        => new EndNode(id);
    public INode CreateSwitch(string id)
        => new SwitchNode(id);

}