namespace DotNetToolbox.Graph;

public class WorkflowBuilder {
    private readonly Dictionary<string, INode> _nodeMap = new();
    private readonly INodeFactory _nodeFactory;
    private INode? _currentNode;

    public WorkflowBuilder(INodeFactory? nodeFactory = null, IKeyProvider<uint>? key = null) {
        _nodeFactory = nodeFactory ?? new NodeFactory(key);
    }

    public INode? Start { get; private set; }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do(string label, Action<Context> action, IPolicy? policy = null) {
        var builder = Do(action, policy);
        _nodeMap[label] = _currentNode;
        return builder;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do(Action<Context> action, IPolicy? policy = null) {
        ConnectNode(_nodeFactory.Do(action, policy: policy));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do<TAction>(string label)
        where TAction : ActionNode<TAction> {
        var builder = Do<TAction>();
        _nodeMap[label] = _currentNode;
        return builder;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do<TAction>()
        where TAction : ActionNode<TAction> {
        ConnectNode(_nodeFactory.Do<TAction>());
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder If(
        string label,
        Func<Context, bool> predicate,
        Action<WorkflowBuilder> setTrueBranch,
        Action<WorkflowBuilder>? setFalseBranch = null) {
        var builder = If(predicate, setTrueBranch, setFalseBranch);
        _nodeMap[label] = _currentNode;
        return builder;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder If(
        Func<Context, bool> predicate,
        Action<WorkflowBuilder> setTrueBranch,
        Action<WorkflowBuilder>? setFalseBranch = null) {
        ConnectNode(_nodeFactory.If(predicate, setTrueBranch, setFalseBranch));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Select(
        string label,
        Func<Context, string> selectPath,
        Action<BranchesBuilder> setPaths) {
        var builder = Select(selectPath, setPaths);
        _nodeMap[label] = _currentNode;
        return builder;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Select(
        Func<Context, string> selectPath,
        Action<BranchesBuilder> setPaths) {
        ConnectNode(_nodeFactory.Select(selectPath, setPaths));
        return this;
    }

    public WorkflowBuilder JumpTo(string label) {
        if (!_nodeMap.TryGetValue(label, out var targetNode))
            throw new InvalidOperationException($"Label '{label}' not found.");

        ConnectNode(targetNode);
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    private void ConnectNode(INode newNode) {
        switch (_currentNode) {
            case IActionNode actionNode:
                actionNode.Next = newNode;
                break;
            case IEntryNode entryNode:
                entryNode.Next = newNode;
                break;
        }
        Start ??= newNode;
        _currentNode = newNode;
    }

    public string BuildGraph() {
        var sb = new StringBuilder();
        sb.AppendLine("graph TD");
        GraphBuilder.Build(sb, [], Start);
        return sb.ToString();
    }
}
