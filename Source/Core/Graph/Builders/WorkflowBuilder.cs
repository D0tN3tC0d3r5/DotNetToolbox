namespace DotNetToolbox.Graph.Builders;

public sealed class WorkflowBuilder
    : IWorkflowBuilder,
      IIfNodeBuilder,
      IElseNodeBuilder,
      ICaseNodeBuilder,
      ICaseIsNodeBuilder {
    private readonly IServiceProvider _services;
    private readonly INodeFactory _nodeFactory;
    private readonly IIdGenerator<uint> _idGenerator;
    private readonly List<INode> _nodes;

    private INode? _first;
    private INode? _current;

    internal WorkflowBuilder(IServiceProvider services, IIdGenerator<uint>? idGenerator, INode? parent, List<INode>? nodes) {
        _services = services;
        _nodeFactory = services.GetRequiredService<INodeFactory>();
        _idGenerator = idGenerator ?? services.GetRequiredService<IIdGenerator<uint>>();
        _nodes = nodes ?? [];
        _first = parent;
    }

    public WorkflowBuilder(IServiceProvider services)
        : this(services, null, null, null) {
    }

    public Result<INode?> Build() {
        var result = Success<INode?>(_first);
        foreach (var node in _nodes)
            result += ConnectNode(node);
        return result;
    }

    internal IWorkflowBuilder Do(Token token,
                                 Action<Context> action,
                                 string? tag,
                                 string? label) {
        var node = _nodeFactory.CreateAction(_idGenerator.Next, action, tag, label);
        node.Token = token;
        _nodes.Add(node);
        return this;
    }

    public IWorkflowBuilder Do(string tag,
                               Action<Context> action,
                               string? label = null)
        => Do(null!, action, IsNotNullOrWhiteSpace(tag), label);

    public IWorkflowBuilder Do(Action<Context> action,
                               string? label = null)
        => Do(null!, action, null, label);

    public IWorkflowBuilder Do<TAction>(string? tag = null, string? label = null)
        where TAction : ActionNode<TAction> {
        var node = _nodeFactory.Create<TAction>(_idGenerator.Next, tag, label);
        _nodes.Add(node);
        return this;
    }

    internal IWorkflowBuilder If(Token token,
                                 Func<Context, bool> predicate,
                                 Action<IIfNodeBuilder> buildBranches,
                                 string? tag,
                                 string? label) {
        var node = _nodeFactory.CreateFork(_idGenerator.Next, predicate, buildBranches, tag, label);
        node.Token = token;
        _nodes.Add(node);
        return this;
    }

    public IWorkflowBuilder If(string tag,
                               Func<Context, bool> predicate,
                               Action<IIfNodeBuilder> buildBranches,
                               string? label = null)
        => If(null!, predicate, buildBranches, IsNotNullOrWhiteSpace(tag), label);

    public IWorkflowBuilder If(Func<Context, bool> predicate,
                               Action<IIfNodeBuilder> buildBranches,
                               string? label = null)
        => If(null!, predicate, buildBranches, null, label);

    internal IWorkflowBuilder Case(Token token,
                                   Func<Context, string> select,
                                   Action<ICaseNodeBuilder> buildChoices,
                                   string? tag,
                                   string? label) {
        var node = _nodeFactory.CreateChoice(_idGenerator.Next, select, buildChoices, tag, label);
        node.Token = token;
        _nodes.Add(node);
        return this;
    }

    public IWorkflowBuilder Case(string tag,
                                 Func<Context, string> select,
                                 Action<ICaseNodeBuilder> buildChoices,
                                 string? label = null)
        => Case(null!, select, buildChoices, IsNotNullOrWhiteSpace(tag), label);

    public IWorkflowBuilder Case(Func<Context, string> select,
                                 Action<ICaseNodeBuilder> buildChoices,
                                 string? label = null)
        => Case(null!, select, buildChoices, null, label);

    internal IWorkflowBuilder Exit(Token token,
                                   int exitCode = 0,
                                   string? tag = null,
                                   string? label = null) {
        var node = _nodeFactory.CreateExit(_idGenerator.Next, exitCode, tag, label);
        node.Token = token;
        _nodes.Add(node);
        return this;
    }

    public IWorkflowBuilder Exit(string? tag = null, int exitCode = 0, string? label = null)
        => Exit(null!, exitCode, tag, label);

    internal IWorkflowBuilder JumpTo(Token token,
                                     string targetTag,
                                     string? tag,
                                     string? label) {
        var node = _nodeFactory.CreateJump(_idGenerator.Next, IsNotNullOrWhiteSpace(targetTag), tag, label);
        node.Token = token;
        _nodes.Add(node);
        return this;
    }

    public IWorkflowBuilder JumpTo(string tag,
                                   string targetNode,
                                   string? label = null)
        => JumpTo(null!, targetNode, IsNotNullOrWhiteSpace(tag), label);

    public IWorkflowBuilder JumpTo(string targetNode,
                                   string? label = null)
        => JumpTo(null!, targetNode, null, label);

    private Result ConnectNode(INode node) {
        _first ??= node;
        var result = _current?.ConnectTo(node) ?? Success();
        _current = node;
        return result;
    }

    public IElseNodeBuilder IsTrue(Action<IWorkflowBuilder> setPath) {
        var trueBuilder = new WorkflowBuilder(_services, _idScope, IsNotNull(_parent), _nodes);
        setPath(trueBuilder);
        ((IfNode)_parent!).IsTrue = trueBuilder.Build();
        return this;
    }

    public INodeBuilder IsFalse(Action<IWorkflowBuilder> setPath) {
        var falseBuilder = new WorkflowBuilder(_services, _idScope, IsNotNull(_parent), _nodes);
        setPath(falseBuilder);
        ((IfNode)_parent).IsFalse = falseBuilder.Build();
        return this;
    }

    public ICaseIsNodeBuilder Is(string key, Action<IWorkflowBuilder> setPath) {
        var branchBuilder = new WorkflowBuilder(_services, _idScope, IsNotNull(_parent), _nodes);
        setPath(branchBuilder);
        ((CaseNode)_parent).Choices[IsNotNullOrWhiteSpace(key)] = branchBuilder.Build();
        return this;
    }

    public INodeBuilder Otherwise(Action<IWorkflowBuilder> setPath) {
        var branchBuilder = new WorkflowBuilder(_services, _idScope, IsNotNull(_parent), _nodes);
        setPath(branchBuilder);
        ((CaseNode)_parent).Choices[string.Empty] = branchBuilder.Build();
        return this;
    }
}
