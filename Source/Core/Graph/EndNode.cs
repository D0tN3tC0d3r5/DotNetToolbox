namespace DotNetToolbox.Graph;

public class EndNode(string id)
    : Node(id) {
    protected override void UpdateState(INode? caller = null, Map? state = null) { }
    protected sealed override INode? SelectExit(INode? caller = null, Map? state = null) => null;
}
