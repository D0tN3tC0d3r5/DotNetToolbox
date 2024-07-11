namespace DotNetToolbox.Graph.Nodes;

public class EndNode(string id)
    : Node(id) {

    protected override Result IsValid() {
        var result = Success();
        if (Exits.Count != 0)
            result += Invalid($"End node '{Id}' can not have any exits.");
        return result;
    }

    protected sealed override INode? GetNext(Map state) => base.GetNext(state);
    protected sealed override void UpdateState(Map state) => base.UpdateState(state);
}
