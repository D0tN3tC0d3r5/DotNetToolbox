namespace DotNetToolbox.Graph;

public sealed class WorkflowBuilder {
    private readonly INodeFactory _nodeFactory;
    private readonly SequentialNodeId _nodeId;
    private readonly Stack<INode> _nodeStack = new();
    private bool _isInElseBranch;
    private bool _isInOtherwiseBranch;

    public WorkflowBuilder(IServiceProvider services, string? id = null, IGuidProvider? guid = null) {
        Id = id ?? (guid ?? GuidProvider.Default).AsSortable.Create().ToString();
        _nodeId = NodeId.FromSequential(Id);
        _nodeFactory = new NodeFactory(services);
    }

    public string Id { get; }
    public INode? First { get; private set; }

    public string BuildGraph() => GraphBuilder.GenerateFrom(First!);

    public WorkflowBuilder Do(string tag,
                              Action<Context> action,
                              string? label = null) {
        ConnectNode(_nodeFactory.CreateAction(_nodeId.Next, action, tag, label));
        return this;
    }

    public WorkflowBuilder Do(Action<Context> action,
                              string? label = null) {
        ConnectNode(_nodeFactory.CreateAction(_nodeId.Next, action, null, label));
        return this;
    }

    public WorkflowBuilder Do<TAction>(string? tag = null, string? label = null)
        where TAction : ActionNode<TAction> {
        ConnectNode(_nodeFactory.Create<TAction>(_nodeId.Next, tag, label));
        return this;
    }

    public WorkflowBuilder If(string tag,
                              Func<Context, bool> predicate,
                              Action<ConditionalNodeBuilder> setConditions,
                              string? label = null) {
        var ifNode = _nodeFactory.CreateFork(_nodeId.Next, predicate, (node, builder) => {
            EnterIfBlock(node);
            setConditions(builder);
            ExitBlock();
        }, tag, label);
        ConnectNode(ifNode);
        return this;
    }

    public WorkflowBuilder If(Func<Context, bool> predicate,
                              Action<ConditionalNodeBuilder> setConditions,
                              string? label = null) {
        var ifNode = _nodeFactory.CreateFork(_nodeId.Next, predicate, (node, builder) => {
            EnterIfBlock(node);
            setConditions(builder);
            ExitBlock();
        }, null, label);
        ConnectNode(ifNode);
        return this;
    }

    public WorkflowBuilder Case(string tag,
                                Func<Context, string> select,
                                Action<BranchingNodeBuilder> setChoices,
                                string? label = null) {
        var caseNode = _nodeFactory.CreateChoice(_nodeId.Next, select, (node, builder) => {
            EnterCaseBlock(node);
            setChoices(builder);
            ExitBlock();
        }, tag, label);
        ConnectNode(caseNode);
        return this;
    }

    public WorkflowBuilder Case(Func<Context, string> select,
                                Action<BranchingNodeBuilder> setChoices,
                                string? label = null) {
        var caseNode = _nodeFactory.CreateChoice(_nodeId.Next, select, (node, builder) => {
            EnterCaseBlock(node);
            setChoices(builder);
            ExitBlock();
        }, null, label);
        ConnectNode(caseNode);
        return this;
    }

    public WorkflowBuilder End(string? tag = null,
                               int exitCode = 0,
                               string? label = null) {
        ConnectNode(_nodeFactory.CreateStop(_nodeId.Next, exitCode, tag, label));
        return this;
    }

    public WorkflowBuilder JumpTo(string targetTag,
                                  string? label = null) {
        ConnectNode(_nodeFactory.CreateJump(_nodeId.Next, targetTag, label));
        return this;
    }

    [MemberNotNull(nameof(First))]
    private void ConnectNode(INode newNode) {
        First ??= newNode;
        if (!_nodeStack.TryPeek(out var node)) {
            _nodeStack.Push(newNode);
            return;
        };

        switch (node) {
            case IActionNode actionNode:
                _nodeStack.Pop();
                actionNode.Next = newNode;
                _nodeStack.Push(newNode);
                break;
            case IConditionalNode ifNode when ifNode.IsTrue is null:
                ifNode.IsTrue = newNode;
                _nodeStack.Push(newNode);
                break;
            case IConditionalNode ifNode when ifNode.IsFalse is null:
                ifNode.IsFalse = newNode;
                _nodeStack.Push(newNode);
                break;
            case IBranchingNode caseNode:
                // This will be handled in the Case method implementation
                break;
        }
    }

    private void EnterIfBlock(IConditionalNode ifNode) {
        _nodeStack.Push(ifNode);
        _isInElseBranch = false;
    }

    private void EnterElseBranch() {
        _isInElseBranch = true;
    }

    private void EnterCaseBlock(IBranchingNode caseNode) {
        _nodeStack.Push(caseNode);
        _isInOtherwiseBranch = false;
    }

    private void EnterOtherwiseBranch() {
        _isInOtherwiseBranch = true;
    }

    public void ExitBlock() {
        if (_nodeStack.Count > 0) {
            _nodeStack.Pop();
        }
    }
}
