namespace DotNetToolbox.Graph.Builders;

public sealed class WorkflowBuilder(IServiceProvider services)
    : IExitBuilder,
      IIfBuilder,
      IElseBuilder,
      IOtherwiseBuilder,
      IActionBuilder {
    private readonly INodeSequence _sequence = services.GetRequiredService<INodeSequence>();
    private readonly List<INode> _nodes = [];

    private INode? _first;
    private INode? _current;

    public INode Build() {
        ConnectJumps();
        return _first ?? new ExitNode(services);
    }

    public IWorkflowBuilder AddNode(INode node) {
        Connect(node);
        return this;
    }

    public IWorkflowBuilder Do<TAction>(params object?[] args)
        where TAction : ActionNode<TAction> {
        AddNode(Node.Create<TAction>(services, args));
        return this;
    }
    public IWorkflowBuilder Do<TAction>(string tag, params object?[] args)
        where TAction : ActionNode<TAction> {
        var node = Node.Create<TAction>(services, args);
        node.Tag = tag;
        AddNode(node);
        return this;
    }

    public IWorkflowBuilder Do(Action<Context> action) {
        AddNode(new ActionNode(action, services));
        return this;
    }
    public IWorkflowBuilder Do(Func<Context, CancellationToken, Task> action) {
        AddNode(new ActionNode(action, services));
        return this;
    }
    public IWorkflowBuilder Do(string tag, Action<Context> action) {
        var node = new ActionNode(action, services) { Tag = tag };
        AddNode(node);
        return this;
    }
    public IWorkflowBuilder Do(string tag, Func<Context, CancellationToken, Task> action) {
        var node = new ActionNode(action, services) { Tag = tag };
        AddNode(node);
        return this;
    }

    public IIfBuilder If(Func<Context, bool> predicate) {
        AddNode(new IfNode(predicate, services));
        return this;
    }
    public IIfBuilder If(Func<Context, CancellationToken, Task<bool>> predicate) {
        AddNode(new IfNode(predicate, services));
        return this;
    }
    public IIfBuilder If(string tag, Func<Context, bool> predicate) {
        var node = new IfNode(predicate, services) { Tag = tag };
        AddNode(node);
        return this;
    }
    public IIfBuilder If(string tag, Func<Context, CancellationToken, Task<bool>> predicate) {
        var node = new IfNode(predicate, services) { Tag = tag };
        AddNode(node);
        return this;
    }
    public IElseBuilder Then(Action<IWorkflowBuilder> setThen) {
        var builder = new WorkflowBuilder(services);
        setThen(builder);
        ((IIfNode)_current!).Then = builder.Build();
        _nodes.AddRange(builder._nodes);
        return this;
    }
    public IWorkflowBuilder Else(Action<IWorkflowBuilder> setElse) {
        var builder = new WorkflowBuilder(services);
        setElse(builder);
        ((IIfNode)_current!).Else = builder.Build();
        _nodes.AddRange(builder._nodes);
        return this;
    }

    public ICaseBuilder Case(Func<Context, string> select) {
        AddNode(new CaseNode(select, services));
        return this;
    }
    public ICaseBuilder Case(Func<Context, CancellationToken, Task<string>> select) {
        AddNode(new CaseNode(select, services));
        return this;
    }
    public ICaseBuilder Case(string tag, Func<Context, string> select) {
        var node = new CaseNode(select, services) { Tag = tag };
        AddNode(node);
        return this;
    }
    public ICaseBuilder Case(string tag, Func<Context, CancellationToken, Task<string>> select) {
        var node = new CaseNode(select, services) { Tag = tag };
        AddNode(node);
        return this;
    }
    public IOtherwiseBuilder Is(string key, Action<IWorkflowBuilder> setCase) {
        var builder = new WorkflowBuilder(services);
        setCase(builder);
        ((ICaseNode)_current!).Choices[IsNotNullOrWhiteSpace(key)] = builder.Build();
        _nodes.AddRange(builder._nodes);
        return this;
    }
    public IWorkflowBuilder Otherwise(Action<IWorkflowBuilder> setOtherwise) {
        var builder = new WorkflowBuilder(services);
        setOtherwise(builder);
        ((ICaseNode)_current!).Choices[string.Empty] = builder.Build();
        _nodes.AddRange(builder._nodes);
        return this;
    }

    public IWorkflowBuilder GoTo(string nodeTag) {
        AddNode(new JumpNode(nodeTag, services));
        return this;
    }

    public IExitBuilder Exit(int exitCode = 0) {
        AddNode(new ExitNode(exitCode, services));
        return this;
    }
    public IExitBuilder Exit(string tag, int exitCode = 0) {
        var node = new ExitNode(exitCode, services) { Tag = tag };
        AddNode(node);
        return this;
    }

    public IWorkflowBuilder WithLabel(string label) {
        _current!.Label = label;
        return this;
    }

    public IWorkflowBuilder WithRetry(IRetryPolicy retry) {
        ((IActionNode)_current!).Retry = retry;
        return this;
    }

    private void Connect(INode node) {
        _nodes.Add(IsNotNull(node));
        _first ??= node;
        _current?.ConnectTo(node);
        _current = node;
    }

    private void ConnectJumps() {
        foreach (var jumpNode in _nodes.OfType<IJumpNode>()) {
            var targetNodeTag = _nodes.Find(n => n.Tag == jumpNode.TargetTag)
                             ?? throw new ValidationException($"Jump target '{jumpNode.TargetTag}' not found.", jumpNode.Token?.ToSource() ?? jumpNode.Tag);
            jumpNode.ConnectTo(targetNodeTag);
        }
    }
}
