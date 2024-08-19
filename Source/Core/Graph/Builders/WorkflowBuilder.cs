namespace DotNetToolbox.Graph.Builders;

public sealed class WorkflowBuilder(IServiceProvider services)
    : IWorkflowBuilder {
    private readonly INodeSequence? _sequence = services.GetService<INodeSequence>()
                                             ?? NodeSequence.Transient;

    private INode? _first;
    private INode? _current;
    private readonly List<INode> _nodes = [];

    public INode Build() {
        ConnectJumps();
        return _first ?? new ExitNode();
    }

    public IWorkflowBuilder AddNode(INode node) {
        Connect(node);
        return this;
    }

    public IWorkflowBuilder Do<TAction>(string? ìd = null,
                                        string? label = null)
        where TAction : ActionNode<TAction> {
        var node = Node.Create<TAction>(services, ìd ?? string.Empty);
        node.Label = label ?? node.Label;
        Connect(node);
        return this;
    }

    public IWorkflowBuilder Do(string ìd,
                               Action<Context> action,
                               string? label = null) {
        var node = new ActionNode(ìd, action);
        node.Label = label ?? node.Label;
        Connect(node);
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
        var ifNode = new IfNode(id, predicate);
        CreateIfNode(ifNode, setThen, setElse, label, services);
        return this;
    }
    public IWorkflowBuilder If(Func<Context, bool> predicate,
                               Action<IWorkflowBuilder> setThen,
                               Action<IWorkflowBuilder>? setElse = null,
                               string? label = null) {
        var ifNode = new IfNode(predicate, _sequence);
        CreateIfNode(ifNode, setThen, setElse, label, services);
        return this;
    }

    public IWorkflowBuilder Case(string id,
                                 Func<Context, string> select,
                                 Dictionary<string, Action<IWorkflowBuilder>> setCases,
                                 Action<IWorkflowBuilder>? setDefault = null,
                                 string? label = null) {
        var node = new CaseNode(id, select);
        SetCaseNode(node, setCases, setDefault, label, services);
        return this;
    }
    public IWorkflowBuilder Case(Func<Context, string> select,
                                 Dictionary<string, Action<IWorkflowBuilder>> setCases,
                                 Action<IWorkflowBuilder>? setDefault = null,
                                 string? label = null)
        => Case(string.Empty, select, setCases, setDefault, label);

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

    private INode CreateIfNode(IfNode node, Action<IWorkflowBuilder> setThen, Action<IWorkflowBuilder>? setElse, string? label, IServiceProvider services) {
        var node = new IfNode(id, _sequence, select);
        node.Label = label ?? node.Label;
        Connect(node);

        var thenBuilder = new WorkflowBuilder(services);
        setThen(thenBuilder);
        node.Then = thenBuilder._first;
        _nodes.AddRange(thenBuilder._nodes);

        if (setElse is null) return;

        var elseBuilder = new WorkflowBuilder(services);
        setElse(elseBuilder);
        node.Else = elseBuilder._first;
        _nodes.AddRange(elseBuilder._nodes);
    }

    private INode CreateCaseNode(string? id,
                                 Func<Context, CancellationToken, Task<string>> select select,
                                 Dictionary<string, Action<IWorkflowBuilder>> setCases,
                                 Action<IWorkflowBuilder>? setDefault,
                                 string? label,
                                 IServiceProvider services) {
        var node = new CaseNode(id, _sequence, select);
        node.Label = label ?? node.Label;
        Connect(node);
        foreach ((var key, var buildChoice) in setCases) {
            var choiceBuilder = new WorkflowBuilder(services);
            buildChoice(choiceBuilder);
            _nodes.AddRange(choiceBuilder._nodes);
            node.Choices[key] = choiceBuilder._first;
        }

        if (setDefault is null) return node;

        var defaultBuilder = new WorkflowBuilder(services);
        setDefault(defaultBuilder);
        _nodes.AddRange(defaultBuilder._nodes);
        node.Choices[string.Empty] = defaultBuilder._first;
        return node;
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
