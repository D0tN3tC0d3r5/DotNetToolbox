namespace DotNetToolbox.AI.Graph;

public class TerminalNode(uint id, INode? caller = null)
    : Node(id, caller) {
    protected override void UpdateState(Map state) { }
    protected sealed override INode? SelectBranch(Map state) => null;
}
