namespace DotNetToolbox.AI.Graph;

public abstract class SwitchNode(string id = 0, IEnumerable<INode?>? entries = null, IEnumerable<INode?>? exits = null)
    : Node(id, entries, exits) {
    protected sealed override void UpdateState(Map state, INode? caller = null) { }
}
