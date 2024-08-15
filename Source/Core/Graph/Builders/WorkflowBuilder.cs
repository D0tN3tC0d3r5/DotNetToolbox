using DotNetToolbox.Graph.Factories;

namespace DotNetToolbox.Graph.Builders;

public interface IWorkflowBuilder
    : INodeBuilder<INode> {
    IWorkflowBuilder Do(string tag, Action<Context> action, string? label = null);
    IWorkflowBuilder Do(Action<Context> action, string? label = null);
    IWorkflowBuilder Do<TAction>(string? tag = null, string? label = null)
        where TAction : ActionNode<TAction>;

    IWorkflowBuilder If(string tag, Func<Context, bool> predicate, Action<IfNodeBuilder> buildBranches, string? label = null);
    IWorkflowBuilder If(Func<Context, bool> predicate, Action<IfNodeBuilder> buildBranches, string? label = null);

    IWorkflowBuilder Case(string tag, Func<Context, string> select, Action<CaseNodeBuilder> buildChoices, string? label = null);
    IWorkflowBuilder Case(Func<Context, string> select, Action<CaseNodeBuilder> buildChoices, string? label = null);

    IWorkflowBuilder JumpTo(string targetTag, string? label = null);
    IWorkflowBuilder Exit(string? tag = null, int exitCode = 0, string? label = null);
}

public sealed class WorkflowBuilder
    : IWorkflowBuilder {
    private readonly INodeFactory _nodeFactory;
    private readonly SequentialNodeId _nodeId;
    private INode? _start;
    private INode? _finish;
    private readonly Dictionary<string, INode> _tagMap;

    public WorkflowBuilder(IServiceProvider services, string? nodeSequenceKey = null, Dictionary<string, INode>? tagMap = null) {
        nodeSequenceKey ??= GuidProvider.Default.Create().ToString();
        _tagMap = tagMap ?? [];
        _nodeId = NodeId.FromSequential(nodeSequenceKey);
        _nodeFactory = new NodeFactory(services, nodeSequenceKey, _tagMap);
    }

    internal INode? BuildBlock() => _start;

    public INode? Build() {
        SetJumps();
        return BuildBlock();
    }

    private void SetJumps() {
        foreach (var jumpNode in _tagMap.Values.OfType<IJumpNode>()) {
            if (!_tagMap.TryGetValue(jumpNode.TargetTag, out var targetNode))
                throw new InvalidOperationException($"Jump target '{jumpNode.TargetTag}' not found.");
            jumpNode.Next = targetNode;
        }
    }

    public IWorkflowBuilder Do(string tag,
                               Action<Context> action,
                               string? label = null) {
        var node = _nodeFactory.CreateAction(_nodeId.Next, action, tag, label);
        ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder Do(Action<Context> action,
                              string? label = null) {
        var node = _nodeFactory.CreateAction(_nodeId.Next, action, null, label);
        ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder Do<TAction>(string? tag = null, string? label = null)
        where TAction : ActionNode<TAction> {
        var node = _nodeFactory.Create<TAction>(_nodeId.Next, tag, label);
        ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder If(string tag,
                               Func<Context, bool> predicate,
                               Action<IfNodeBuilder> buildBranches,
                               string? label = null) {
        var node = _nodeFactory.CreateFork(_nodeId.Next, predicate, buildBranches, tag, label);
        ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder If(Func<Context, bool> predicate,
                               Action<IfNodeBuilder> buildBranches,
                               string? label = null) {
        var node = _nodeFactory.CreateFork(_nodeId.Next, predicate, buildBranches, null, label);
        ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder Case(string tag,
                                 Func<Context, string> select,
                                 Action<CaseNodeBuilder> buildChoices,
                                 string? label = null) {
        var node = _nodeFactory.CreateChoice(_nodeId.Next, select, buildChoices, tag, label);
        ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder Case(Func<Context, string> select,
                                 Action<CaseNodeBuilder> buildChoices,
                                 string? label = null) {
        var node = _nodeFactory.CreateChoice(_nodeId.Next, select, buildChoices, null, label);
        ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder Exit(string? tag = null, int exitCode = 0, string? label = null) {
        var node = _nodeFactory.CreateExit(_nodeId.Next, exitCode, tag, label);
        ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder JumpTo(string targetTag, string? label = null) {
        var node = _nodeFactory.CreateJump(_nodeId.Next, targetTag, label);
        ConnectNode(node);
        return this;
    }

    private void ConnectNode(INode newNode) {
        if (!_tagMap.TryAdd(newNode.Tag, newNode))
            throw new InvalidOperationException($"Node with tag {newNode.Tag} already exists.");
        _start ??= newNode;
        _finish?.ConnectTo(newNode);
        _finish = newNode;
    }
}
