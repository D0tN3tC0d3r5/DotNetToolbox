namespace DotNetToolbox.Graph;

public class WorkflowBuilder {
    private readonly HashSet<INode?> _nodes;
    private readonly INodeFactory _nodeFactory = new NodeFactory();
    private INode? _currentNode;

    public WorkflowBuilder(HashSet<INode?>? nodes = null) {
        if (nodes is null) NodeId.Reset();
        _nodes = nodes ?? [];
    }

    public INode? Start { get; private set; }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do(string label, Action<Context> action, IPolicy? policy = null) {
        ConnectNode(_nodeFactory.CreateAction(label, action, policy));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do(Action<Context> action, IPolicy? policy = null) {
        ConnectNode(_nodeFactory.CreateAction(action, policy));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do<TAction>(string label)
        where TAction : ActionNode<TAction> {
        ConnectNode(_nodeFactory.CreateAction<TAction>(label));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Do<TAction>()
        where TAction : ActionNode<TAction> {
        ConnectNode(_nodeFactory.CreateAction<TAction>());
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder If(string label,
                              Func<Context, bool> predicate,
                              Action<WorkflowBuilder> setTrueBranch,
                              Action<WorkflowBuilder>? setFalseBranch = null) {
        ConnectNode(_nodeFactory.CreateFork(label, predicate, setTrueBranch, setFalseBranch, _nodes));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder If(Func<Context, bool> predicate,
                              Action<WorkflowBuilder> setTrueBranch,
                              Action<WorkflowBuilder>? setFalseBranch = null) {
        ConnectNode(_nodeFactory.CreateFork(predicate, setTrueBranch, setFalseBranch, _nodes));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Select(string label,
                                  Func<Context, string> select,
                                  Action<BranchesBuilder> setChoices) {
        ConnectNode(_nodeFactory.CreateChoice(label, select, setChoices, _nodes));
        return this;
    }

    [MemberNotNull(nameof(Start))]
    [MemberNotNull(nameof(_currentNode))]
    public WorkflowBuilder Select(Func<Context, string> selectPath,
                                  Action<BranchesBuilder> setPaths) {
        ConnectNode(_nodeFactory.CreateChoice(selectPath, setPaths, _nodes));
        return this;
    }

    public WorkflowBuilder JumpTo(string label) {
        var node = _nodes.FirstOrDefault(n => n?.Label == IsNotNull(label))
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
        _nodes.Add(_currentNode);
    }

    public string BuildGraph()
        => GraphBuilder.GenerateFrom(Start!);
}
