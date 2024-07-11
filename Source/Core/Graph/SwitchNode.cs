namespace DotNetToolbox.Graph;

public class SwitchNode(string id, Func<INode?, Map, INode?>? selectExit = null)
    : Node(id) {
    protected sealed override void UpdateState(INode? caller = null, Map? state = null) { }
    protected override INode? SelectExit(INode? caller = null, Map? state = null)
        => IsNotNull(selectExit)(caller, state);
}
