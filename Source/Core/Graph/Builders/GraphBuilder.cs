namespace DotNetToolbox.Graph.Builders;

public sealed class GraphBuilder {
    private readonly HashSet<INode> _visited = [];
    private readonly StringBuilder _stringBuilder = new();

    private GraphBuilder() { }

    public static string BuildFrom(INode node) {
        var builder = new GraphBuilder();
        return builder.Build(IsNotNull(node));
    }

    private string Build(INode? node, INode? previous = null, string? label = null) {
        if (node is null)
            return _stringBuilder.ToString();
        if (previous is not null)
            AddEdge(previous.Number, node.Number, label);
        else
            _stringBuilder.AppendLine("flowchart TD");
        if (!_visited.Add(node))
            return _stringBuilder.ToString();
        _stringBuilder.AppendLine($"{node.Number}[\"{node.Label}\"]");

        switch (node) {
            case IActionNode actionNode:
                Build(actionNode.Next, actionNode);
                break;

            case IIfNode ifNode:
                Build(ifNode.Then, ifNode, "True");
                Build(ifNode.Else, ifNode, "False");
                break;

            case ICaseNode mapNode:
                foreach ((var name, var branch) in mapNode.Choices)
                    Build(branch, mapNode, name);
                break;
        }
        return _stringBuilder.ToString();
    }

    private void AddEdge(uint fromId, uint toId, string? label = null) {
        if (label is null)
            _stringBuilder.AppendLine($"{fromId} --> {toId}");
        else _stringBuilder.AppendLine($"{fromId} --> |{label}| {toId}");
    }
}
