namespace DotNetToolbox.Graph;

public sealed class WorkflowBuilder {
    private readonly INodeFactory _nodeFactory;
    private INode? _currentNode;
    private readonly SequentialNodeId _nodeId;

    public WorkflowBuilder(IServiceProvider services, string? id = null, HashSet<INode>? nodes = null, IGuidProvider? guid = null) {
        Id = id ?? (guid ?? GuidProvider.Default).AsSortable.Create().ToString();
        _nodeId = NodeId.FromSequential(Id);
        _nodeFactory = new NodeFactory(services);
        Nodes = nodes ?? [];
    }

    public string Id { get; }
    public HashSet<INode> Nodes { get; }
    public INode? Start { get; private set; }

    [MemberNotNull(nameof(Start))]
    public WorkflowBuilder Do(string tag, Action<Context> action, string? label = null) {
        ConnectNode(_nodeFactory.CreateAction(_nodeId.Next, action, tag, label));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    public WorkflowBuilder Do(Action<Context> action, string? label = null) {
        ConnectNode(_nodeFactory.CreateAction(_nodeId.Next, action, null, label));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    public WorkflowBuilder Do<TAction>(string? tag = null, string? label = null)
        where TAction : ActionNode<TAction> {
        ConnectNode(_nodeFactory.Create<TAction>(_nodeId.Next, tag, label));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    public WorkflowBuilder If(string tag,
                              Func<Context, bool> predicate,
                              Action<ConditionalNodeBuilder> setConditions,
                              string? label = null) {
        ConnectNode(_nodeFactory.CreateFork(_nodeId.Next, predicate, this, setConditions, tag, label));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    public WorkflowBuilder If(Func<Context, bool> predicate,
                              Action<ConditionalNodeBuilder> setConditions,
                              string? label = null) {
        ConnectNode(_nodeFactory.CreateFork(_nodeId.Next, predicate, this, setConditions, null, label));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    public WorkflowBuilder Case(string tag,
                                Func<Context, string> select,
                                Action<BranchingNodeBuilder> setChoices,
                                string? label = null) {
        ConnectNode(_nodeFactory.CreateChoice(_nodeId.Next, select, this, setChoices, tag, label));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    public WorkflowBuilder Case(Func<Context, string> select,
                                Action<BranchingNodeBuilder> setChoices,
                                string? label = null) {
        ConnectNode(_nodeFactory.CreateChoice(_nodeId.Next, select, this, setChoices, null, label));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    public WorkflowBuilder End(string? tag = null, int exitCode = 0, string? label = null) {
        ConnectNode(_nodeFactory.CreateStop(_nodeId.Next, exitCode, tag, label));
        return this;
    }

    public WorkflowBuilder JumpTo(string target) {
        var node = Nodes.First(n => n.Tag == IsNotNull(target));
        ConnectNode(node);
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    private void ConnectNode(INode newNode) {
        Start ??= newNode;
        switch (_currentNode) {
            case IActionNode actionNode:
                actionNode.Next = newNode;
                break;
        }
        _currentNode = newNode;
        Nodes.Add(_currentNode);
    }

    public string BuildGraph()
        => GraphBuilder.GenerateFrom(Start!);
}
