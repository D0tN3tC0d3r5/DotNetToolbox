namespace DotNetToolbox.Graph;

public class GraphBuilder(StringBuilder stringBuilder, HashSet<INode> visited) {
    internal void Build(INode? node, INode? previous = null, string? label = null) {
        if (node is null) return;
        if (!visited.Add(node)) return;
        if (previous is not null)
            AddEdge(previous.Id, node.Id, label);
        stringBuilder.AppendLine($"    {node.Id}[\"{GetNodeLabel(node)}\"]");

        switch (node) {
            case IEntryNode entryNode:
                Build(entryNode.Next, entryNode);
                return;

            case IActionNode actionNode:
                Build(actionNode.Next, actionNode);
                return;

            case IConditionalNode ifNode:
                Build(ifNode.True, ifNode, "True");
                Build(ifNode.False, ifNode, "False");
                return;

            case IBranchingNode mapNode:
                foreach (var (name, branch) in mapNode.Branches) {
                    Build(branch, mapNode, name);
                }
                return;
        }
    }

    private void AddEdge(string from, string to, string? label = null) {
        if (label is null)
            stringBuilder.AppendLine($"    {from} --> {to}");
        else stringBuilder.AppendLine($"    {from} --> |{label}| {to}");
    }

    private static string GetNodeLabel(INode node)
        => node switch {
            IEntryNode => "Start",
            IActionNode => "Action",
            IConditionalNode => "If",
            IBranchingNode => "Map",
            _ => "Unknown"
        };
}
