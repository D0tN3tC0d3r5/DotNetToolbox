namespace DotNetToolbox.Graph;

public sealed class WorkflowBuilder {
    private readonly INodeFactory _nodeFactory;
    private readonly SequentialNodeId _nodeId;

    public WorkflowBuilder(IServiceProvider services, string? id = null, IGuidProvider? guid = null) {
        Id = id ?? (guid ?? GuidProvider.Default).AsSortable.Create().ToString();
        _nodeId = NodeId.FromSequential(Id);
        _nodeFactory = new NodeFactory(services);
    }

    public string Id { get; }
    public INode? First { get; private set; }
    public INode? Last { get; private set; }

    public string BuildGraph()
        => GraphBuilder.GenerateFrom(First!);

    [MemberNotNull(nameof(First))]
    public WorkflowBuilder Do(string tag, Action<Context> action, string? label = null) {
        ConnectNode(_nodeFactory.CreateAction(_nodeId.Next, action, tag, label));
        return this;
    }

    [MemberNotNull(nameof(First))]
    public WorkflowBuilder Do(Action<Context> action, string? label = null) {
        ConnectNode(_nodeFactory.CreateAction(_nodeId.Next, action, null, label));
        return this;
    }

    [MemberNotNull(nameof(First))]
    public WorkflowBuilder Do<TAction>(string? tag = null, string? label = null)
        where TAction : ActionNode<TAction> {
        ConnectNode(_nodeFactory.Create<TAction>(_nodeId.Next, tag, label));
        return this;
    }

    [MemberNotNull(nameof(First))]
    public WorkflowBuilder If(string tag,
                              Func<Context, bool> predicate,
                              Action<ConditionalNodeBuilder> setConditions,
                              string? label = null) {
        ConnectNode(_nodeFactory.CreateFork(_nodeId.Next, predicate, this, setConditions, tag, label));
        return this;
    }

    [MemberNotNull(nameof(First))]
    public WorkflowBuilder If(Func<Context, bool> predicate,
                              Action<ConditionalNodeBuilder> setConditions,
                              string? label = null) {
        ConnectNode(_nodeFactory.CreateFork(_nodeId.Next, predicate, this, setConditions, null, label));
        return this;
    }

    [MemberNotNull(nameof(First))]
    public WorkflowBuilder Case(string tag,
                                Func<Context, string> select,
                                Action<BranchingNodeBuilder> setChoices,
                                string? label = null) {
        ConnectNode(_nodeFactory.CreateChoice(_nodeId.Next, select, this, setChoices, tag, label));
        return this;
    }

    [MemberNotNull(nameof(First))]
    public WorkflowBuilder Case(Func<Context, string> select,
                                Action<BranchingNodeBuilder> setChoices,
                                string? label = null) {
        ConnectNode(_nodeFactory.CreateChoice(_nodeId.Next, select, this, setChoices, null, label));
        return this;
    }

    [MemberNotNull(nameof(First))]
    public WorkflowBuilder End(string? tag = null, int exitCode = 0, string? label = null) {
        ConnectNode(_nodeFactory.CreateStop(_nodeId.Next, exitCode, tag, label));
        return this;
    }

    public WorkflowBuilder JumpTo(string targetTag, string? label = null) {
        ConnectNode(_nodeFactory.CreateJump(_nodeId.Next, targetTag, label));
        return this;
    }

    [MemberNotNull(nameof(First))]
    private void ConnectNode(INode newNode) {
        First ??= newNode;
        if (Last is IActionNode) {
            newNode.Next = Last;
            Last = newNode;
        }
        if (Last is IConditionalNode ifNode) return;
        if (Last is IBranchingNode caseNode) return;
        if (Last is IJumpNode jumpNode) return;
        if (Last is ITerminationNode endNode) return;
    }

    //[MemberNotNull(nameof(First))]
    //private void ConnectNode(INode newNode) {
    //    First ??= newNode;
    //    if (_nodeStack.Count > 0) {
    //        var parentNode = _nodeStack.Pop();
    //        if (parentNode is IBranchingNode branchingNode) {
    //            foreach (var choice in branchingNode.Choices) {
    //                if (choice.Key == newNode.Tag) {
    //                    throw new InvalidOperationException($"Branch with tag '{newNode.Tag}' already exists.");
    //                } else {
    //                    branchingNode.Choices[newNode.Tag] = newNode;
    //                }
    //            }
    //            newNode.Next = branchingNode.Next;
    //        } else if (parentNode is IConditionalNode conditionalNode) {
    //            if (conditionalNode.IsTrue == null) { 
    //                conditionalNode.IsTrue = newNode;
    //            } else if (conditionalNode.IsFalse == null) {
    //                conditionalNode.IsFalse = newNode;
    //            } else { 
    //                throw new InvalidOperationException("Conditional node already has two paths.";
    //            }
    //            newNode.Next = conditionalNode.Next;
    //        } else if (parentNode is IActionNode actionNode) {
    //            actionNode.Next = newNode; 
    //        } else if (parentNode is ITerminationNode terminationNode) {
    //            throw new Exception("Cannot connect a node to a termination node.");
    //        } else if (parentNode is IJumpNode jumpNode) {
    //                throw new Exception("Cannot connect a node to a jump node.");
    //        } else if (parentNode is INode node) {
    //                    node.Next = newNode;
    //        } else { 
    //            throw new InvalidOperationException($"Unknown node type: {parentNode.GetType().Name}");
    //        }
    //        newNode.Next = parentNode.Next;
    //    }
    //    _nodeStack.Push(newNode);
    //    Nodes.Add(newNode);
    //}
}
