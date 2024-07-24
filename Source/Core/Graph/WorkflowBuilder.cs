namespace DotNetToolbox.Graph;

public sealed class WorkflowBuilder {
    private readonly INodeFactory _nodeFactory;
    private INode? _currentNode;
    private readonly SequentialNodeId _nodeId;

    public WorkflowBuilder(string? id = null, HashSet<INode?>? nodes = null) {
        Id = id ?? GuidProvider.Default.AsSortable.Create().ToString();
        _nodeId = NodeId.FromSequential(Id);
        _nodeFactory = new NodeFactory();
        Nodes = nodes ?? [];
    }

    public string? Id { get; }
    public HashSet<INode?> Nodes { get; }
    public INode? Start { get; private set; }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do(string label, Action<Context> action, IPolicy? policy = null) {
        ConnectNode(_nodeFactory.CreateAction(_nodeId.Next, label, action, policy));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do(Action<Context> action, IPolicy? policy = null) {
        ConnectNode(_nodeFactory.CreateAction(_nodeId.Next, action, policy));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do<TAction>(string label)
        where TAction : ActionNode<TAction> {
        ConnectNode(_nodeFactory.CreateAction<TAction>(_nodeId.Next, label));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do<TAction>()
        where TAction : ActionNode<TAction> {
        ConnectNode(_nodeFactory.CreateAction<TAction>(_nodeId.Next));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder If(string label,
                              Func<Context, bool> predicate,
                              Action<WorkflowBuilder> setTrueBranch,
                              Action<WorkflowBuilder>? setFalseBranch = null) {
        ConnectNode(_nodeFactory.CreateFork(_nodeId.Next, label, predicate, this, setTrueBranch, setFalseBranch));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder If(Func<Context, bool> predicate,
                              Action<WorkflowBuilder> setTrueBranch,
                              Action<WorkflowBuilder>? setFalseBranch = null) {
        ConnectNode(_nodeFactory.CreateFork(_nodeId.Next, predicate, this, setTrueBranch, setFalseBranch));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder When(string label,
                                Func<Context, string> select,
                                Action<BranchesBuilder> setChoices) {
        ConnectNode(_nodeFactory.CreateChoice(_nodeId.Next, label, select, this, setChoices));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder When(Func<Context, string> selectPath,
                                Action<BranchesBuilder> setPaths) {
        ConnectNode(_nodeFactory.CreateChoice(_nodeId.Next, selectPath, this, setPaths));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder End(string label) {
        ConnectNode(_nodeFactory.CreateStop(_nodeId.Next, label));
        return this;
    }

    public WorkflowBuilder JumpTo(string label) {
        var node = Nodes.FirstOrDefault(n => n?.Label == IsNotNull(label))
            ?? throw new InvalidOperationException($"Label '{label}' not found.");
        _currentNode!.Next = node;
        _currentNode = node;
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
            case IStartingNode entryNode:
                entryNode.Next = newNode;
                break;
        }
        _currentNode = newNode;
        Nodes.Add(_currentNode);
    }

    public string BuildGraph()
        => GraphBuilder.GenerateFrom(Start!);
}
