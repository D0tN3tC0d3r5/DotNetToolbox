namespace DotNetToolbox.Graph.Builders;

public sealed class WorkflowGraph
    : IWorkflowGraphSettings {
    private readonly HashSet<INode> _visited = [];
    private readonly StringBuilder _stringBuilder = new();
    private GraphFormat _format = GraphFormat.Default;
    private GraphDirection _direction = GraphDirection.Vertical;

    public static string Draw(INode node, Action<IWorkflowGraphSettings>? configure = null) {
        var builder = new WorkflowGraph();
        configure?.Invoke(builder);
        return builder.WriteCode(IsNotNull(node));
    }

    public IWorkflowGraphSettings Format(GraphFormat format) {
        _format = format;
        return this;
    }

    public IWorkflowGraphSettings Direction(GraphDirection direction) {
        _direction = direction;
        return this;
    }

    private string WriteCode(INode node) {
        DrawHeader();
        DrawBody(0, node);
        return _stringBuilder.ToString();
    }

    private void DrawBody(int level, INode? node) {
        if (node is null) return;
        if (!_visited.Add(node)) return;
        var nextLevel = level + 1;
        if (_format is not GraphFormat.Indented) {
            nextLevel = level = 0;
        }
        DrawBox(level, node);
        switch (node) {
            case IActionNode actionNode:
                DrawEdge(level, actionNode, actionNode.Next);
                DrawBody(level, actionNode.Next);
                break;

            case IIfNode ifNode when _format is GraphFormat.GroupedEdges:
                DrawEdge(level, ifNode, ifNode.Then, "True");
                DrawEdge(level, ifNode, ifNode.Else, "False");
                DrawBody(level, ifNode.Then);
                DrawBody(level, ifNode.Else);
                break;
            case IIfNode ifNode:
                DrawEdge(level, ifNode, ifNode.Then, "True");
                DrawBody(nextLevel, ifNode.Then);
                DrawEdge(level, ifNode, ifNode.Else, "False");
                DrawBody(nextLevel, ifNode.Else);
                break;
            case ICaseNode caseNode when _format is GraphFormat.GroupedEdges:
                foreach ((var key, var choice) in caseNode.Choices)
                    DrawEdge(level, caseNode, choice, key);
                foreach ((_, var choice) in caseNode.Choices) {
                    DrawBody(level, choice);
                }
                break;
            case ICaseNode caseNode:
                foreach ((var key, var choice) in caseNode.Choices) {
                    DrawEdge(level, caseNode, choice, key);
                    DrawBody(nextLevel, choice);
                }
                break;
            case IJumpNode jumpNode:
                DrawEdge(level, jumpNode, jumpNode.Next);
                break;
            default:
                DrawBody(level, node.Next);
                break;
        }
    }

    private void DrawHeader() {
        var direction = _direction == GraphDirection.Vertical ? "TD" : "LR";
        _stringBuilder.AppendLine($"flowchart {direction}");
    }

    private void DrawBox(int level, INode node) {
        _stringBuilder.Append(' ', level * 4);
        _stringBuilder.AppendLine($"{node.Id}[\"{node.Tag ?? node.Label}\"]");
    }

    private void DrawEdge(int level, INode? from, INode? to, string? label = null) {
        if (from is null) return;
        if (to is null) return;
        var labelText = label is null ? string.Empty : $" |{label}|";
        _stringBuilder.Append(' ', level * 4);
        _stringBuilder.AppendLine($"{from.Id} -->{labelText} {to.Id}");
    }
}
