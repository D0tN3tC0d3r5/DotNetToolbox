namespace DotNetToolbox.Graph.Nodes;

public class EndNode(string id)
    : Node(id) {
    protected override Result IsValid() {
        var result = Success();
        if (Paths.Count != 0)
            result += Invalid($"Void node '{Id}' can not have any exits.");
        return result;
    }

    protected sealed override INode? GetNext(Context state) => base.GetNext(state);
    protected sealed override void UpdateState(Context state) => base.UpdateState(state);
}
