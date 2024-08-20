namespace DotNetToolbox.Graph.Builders;

public sealed class WorkflowBuilder(IServiceProvider services)
    : IWorkflowBuilder {
    private readonly IServiceProvider _services = services;
    private readonly INodeSequence _sequence = services.GetService<INodeSequence>()
                                             ?? NodeSequence.Singleton;
    private readonly List<INode> _nodes = [];

    private INode? _first;
    private INode? _current;

    public INode Build() {
        ConnectJumps();
        return _first ?? new ExitNode();
    }

    public IWorkflowBuilder AddNode(INode node) {
        Connect(node);
        return this;
    }

    public IWorkflowBuilder Do<TAction>(params object?[] args)
        where TAction : ActionNode<TAction> {
        var node = Node.Create<TAction>(_services, args);
        //node.Label = label ?? node.Label;
        AddNode(node);
        return this;
    }

    public IWorkflowBuilder Do(string id,
                               Action<Context> action,
                               string? label = null) {
        CreateActionNode(id,
                        (ctx, ct) => Task.Run(() => action(ctx), ct),
                        label);
        return this;
    }
    public IWorkflowBuilder Do(Action<Context> action,
                               string? label = null)
        => Do(string.Empty, action, label);

    public IWorkflowBuilder If(string id,
                               Func<Context, bool> predicate,
                               Action<IWorkflowBuilder> setThen,
                               Action<IWorkflowBuilder>? setElse = null,
                               string? label = null) {
        CreateIfNode(id,
                     (ctx, ct) => Task.Run(() => predicate(ctx), ct),
                     setThen,
                     setElse,
                     label);
        return this;
    }
    public IWorkflowBuilder If(Func<Context, bool> predicate,
                               Action<IWorkflowBuilder> setThen,
                               Action<IWorkflowBuilder>? setElse = null,
                               string? label = null) {
        CreateIfNode(null,
                     (ctx, ct) => Task.Run(() => predicate(ctx), ct),
                     setThen,
                     setElse,
                     label);
        return this;
    }

    public IWorkflowBuilder Case(string id,
                                 Func<Context, string> select,
                                 Dictionary<string, Action<IWorkflowBuilder>> setCases,
                                 Action<IWorkflowBuilder>? setDefault = null,
                                 string? label = null) {
        CreateCaseNode(id,
                       (ctx, ct) => Task.Run(() => select(ctx), ct),
                       setCases,
                       setDefault,
                       label);
        return this;
    }
    public IWorkflowBuilder Case(Func<Context, string> select,
                                 Dictionary<string, Action<IWorkflowBuilder>> setCases,
                                 Action<IWorkflowBuilder>? setDefault = null,
                                 string? label = null) {
        CreateCaseNode(null,
                       (ctx, ct) => Task.Run(() => select(ctx), ct),
                       setCases,
                       setDefault,
                       label);
        return this;
    }

    public IWorkflowBuilder JumpTo(string targetNodeId,
                                   string? label = null) {
        var node = new JumpNode(targetNodeId, _sequence);
        node.Label = label ?? node.Label;
        Connect(node);
        return this;
    }

    public IWorkflowBuilder Exit(string id,
                                 int exitCode = 0,
                                 string? label = null) {
        var node = new ExitNode(id, exitCode);
        node.Label = label ?? node.Label;
        Connect(node);
        return this;
    }
    public IWorkflowBuilder Exit(int exitCode = 0,
                                 string? label = null) {
        var node = new ExitNode(exitCode, _sequence);
        node.Label = label ?? node.Label;
        Connect(node);
        return this;
    }

    private void CreateActionNode(string? id,
                                  Func<Context, CancellationToken, Task> action,
                                  string? label) {
        var policy = _services.GetService<IPolicy>() ?? Policy.Default;
        var node = new ActionNode(id, _sequence, policy, action);
        node.Label = label ?? node.Label;
        Connect(node);
    }

    private void CreateIfNode(string? id,
                              Func<Context, CancellationToken, Task<bool>> predicate,
                              Action<IWorkflowBuilder> setThen,
                              Action<IWorkflowBuilder>? setElse,
                              string? label) {
        var node = new IfNode(id, _sequence, predicate);
        node.Label = label ?? node.Label;
        Connect(node);

        var thenBuilder = new WorkflowBuilder(_services);
        setThen(thenBuilder);
        node.Then = thenBuilder._first;
        _nodes.AddRange(thenBuilder._nodes);

        if (setElse is null) return;

        var elseBuilder = new WorkflowBuilder(_services);
        setElse(elseBuilder);
        node.Else = elseBuilder._first;
        _nodes.AddRange(elseBuilder._nodes);
    }

    private void CreateCaseNode(string? id,
                                Func<Context, CancellationToken, Task<string>> select,
                                Dictionary<string, Action<IWorkflowBuilder>> setCases,
                                Action<IWorkflowBuilder>? setDefault,
                                string? label) {
        var node = new CaseNode(id, _sequence, select);
        node.Label = label ?? node.Label;
        Connect(node);
        foreach ((var key, var buildChoice) in setCases) {
            var choiceBuilder = new WorkflowBuilder(_services);
            buildChoice(choiceBuilder);
            _nodes.AddRange(choiceBuilder._nodes);
            node.Choices[key] = choiceBuilder._first;
        }

        if (setDefault is null) return;

        var defaultBuilder = new WorkflowBuilder(_services);
        setDefault(defaultBuilder);
        _nodes.AddRange(defaultBuilder._nodes);
        node.Choices[string.Empty] = defaultBuilder._first;
    }

    private void Connect(INode node) {
        _nodes.Add(IsNotNull(node));
        _first ??= node;
        _current?.ConnectTo(node);
        _current = node;
    }

    private void ConnectJumps() {
        foreach (var jumpNode in _nodes.OfType<IJumpNode>()) {
            var targetNodeTag = _nodes.Find(n => n.Id == jumpNode.TargetTag)
                             ?? throw new ValidationException($"Jump target '{jumpNode.TargetTag}' not found.", jumpNode.Token?.ToSource() ?? jumpNode.Id);
            jumpNode.ConnectTo(targetNodeTag);
        }
    }
}
