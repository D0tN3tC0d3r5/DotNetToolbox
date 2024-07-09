namespace DotNetToolbox.AI.Graph;

public class EndNode(string id, IEnumerable<INode?>? entries = null)
    : Node(id, entries) {
    protected override void UpdateState(Map state, INode? caller = null) { }
    protected sealed override INode? SelectExit(Map state, INode? caller = null) => null;
}
