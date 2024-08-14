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
    IWorkflowBuilder End(string? tag = null, int exitCode = 0, string? label = null);
}

public sealed class WorkflowBuilder
    : IWorkflowBuilder {
    private readonly INodeFactory _nodeFactory;
    private readonly SequentialNodeId _nodeId;
    private readonly INode _start;
    private INode _finish;

    public WorkflowBuilder(IServiceProvider services) {
        var key = GuidProvider.Default.Create().ToString();
        _nodeFactory = new NodeFactory(services);
        _nodeId = NodeId.FromSequential(key);
        _start = _nodeFactory.CreateStop(_nodeId.Next);
        _finish = _start;
    }

    public INode Build() => _start;

    public IWorkflowBuilder Do(string tag,
                              Action<Context> action,
                              string? label = null) {
        ConnectNode(_nodeFactory.CreateAction(_nodeId.Next, action, tag, label));
        return this;
    }

    public IWorkflowBuilder Do(Action<Context> action,
                              string? label = null) {
        ConnectNode(_nodeFactory.CreateAction(_nodeId.Next, action, null, label));
        return this;
    }

    public IWorkflowBuilder Do<TAction>(string? tag = null, string? label = null)
        where TAction : ActionNode<TAction> {
        ConnectNode(_nodeFactory.Create<TAction>(_nodeId.Next, tag, label));
        return this;
    }

    public IWorkflowBuilder If(string tag,
                              Func<Context, bool> predicate,
                              Action<IfNodeBuilder> buildBranches,
                              string? label = null) {
        var ifNode = _nodeFactory.CreateFork(_nodeId.Next, predicate, buildBranches, tag, label);
        ConnectNode(ifNode);
        return this;
    }

    public IWorkflowBuilder If(Func<Context, bool> predicate,
                              Action<IfNodeBuilder> buildBranches,
                              string? label = null) {
        var ifNode = _nodeFactory.CreateFork(_nodeId.Next, predicate, buildBranches, null, label);
        ConnectNode(ifNode);
        return this;
    }

    public IWorkflowBuilder Case(string tag,
                                Func<Context, string> select,
                                Action<CaseNodeBuilder> buildChoices,
                                string? label = null) {
        var caseNode = _nodeFactory.CreateChoice(_nodeId.Next, select, buildChoices, tag, label);
        ConnectNode(caseNode);
        return this;
    }

    public IWorkflowBuilder Case(Func<Context, string> select,
                                Action<CaseNodeBuilder> buildChoices,
                                string? label = null) {
        var caseNode = _nodeFactory.CreateChoice(_nodeId.Next, select, buildChoices, null, label);
        ConnectNode(caseNode);
        return this;
    }

    public IWorkflowBuilder End(string? tag = null, int exitCode = 0, string? label = null) {
        ConnectNode(_nodeFactory.CreateStop(_nodeId.Next, exitCode, tag, label));
        return this;
    }

    public IWorkflowBuilder JumpTo(string targetTag, string? label = null) {
        ConnectNode(_nodeFactory.CreateJump(_nodeId.Next, targetTag, label));
        return this;
    }

    private void ConnectNode(INode newNode) {
        _finish.ConnectTo(newNode);
        _finish = newNode;
    }
}
