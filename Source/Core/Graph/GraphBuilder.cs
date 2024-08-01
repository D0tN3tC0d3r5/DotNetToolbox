namespace DotNetToolbox.Graph;

public sealed class GraphBuilder {
    private readonly HashSet<INode> _visited = [];
    private readonly StringBuilder _stringBuilder = new();

    private GraphBuilder() { }

    public static string GenerateFrom(INode node) {
        var builder = new GraphBuilder();
        return builder.Build(IsNotNull(node));
    }

    private string Build(INode? node, INode? previous = null, string? label = null) {
        if (node is null)
            return _stringBuilder.ToString();
        if (previous is not null)
            AddEdge(previous.Id, node.Id, label);
        else
            _stringBuilder.AppendLine("flowchart TD");
        if (!_visited.Add(node))
            return _stringBuilder.ToString();
        _stringBuilder.AppendLine($"{node.Id}[\"{node.Label}\"]");

        switch (node) {
            case IStartingNode entryNode:
                Build(entryNode.Next, entryNode);
                return _stringBuilder.ToString();

            case IActionNode actionNode:
                Build(actionNode.Next, actionNode);
                return _stringBuilder.ToString();

            case IConditionalNode ifNode:
                Build(ifNode.IsTrue, ifNode, "True");
                Build(ifNode.IsFalse, ifNode, "False");
                Build(ifNode.Next, ifNode);
                return _stringBuilder.ToString();

            case IBranchingNode mapNode:
                foreach (var (name, branch) in mapNode.Choices)
                    Build(branch, mapNode, name);
                Build(mapNode.Next, mapNode);
                return _stringBuilder.ToString();
        }
        return _stringBuilder.ToString();
    }

    private void AddEdge(uint fromId, uint toId, string? label = null) {
        if (label is null)
            _stringBuilder.AppendLine($"{fromId} --> {toId}");
        else _stringBuilder.AppendLine($"{fromId} --> |{label}| {toId}");
    }
}
