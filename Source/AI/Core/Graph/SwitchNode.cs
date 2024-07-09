namespace DotNetToolbox.AI.Graph;

public abstract class SwitchNode(uint id = 0, INode? caller = null, IEnumerable<INode?>? branches = null)
    : Node(id, caller, branches) {
    protected sealed override void UpdateState(Map state) { }
}
