namespace DotNetToolbox.Graph.Builders;

public sealed class WorkflowBuilder(IServiceProvider services)
    : IWorkflowBuilder {
    private readonly INodeFactory _nodeFactory = services.GetRequiredService<INodeFactory>();

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
        var node = _nodeFactory.Create<TAction>(ìd ?? string.Empty);
        node.Label = label ?? node.Label;
        Connect(node);
        return this;
    }

    public IWorkflowBuilder Do(string ìd,
                               Action<Context> action,
                               string? label = null) {
        var node = _nodeFactory.CreateAction(ìd, action);
        node.Label = label ?? node.Label;
        Connect(node);
        return this;
    }
    public IWorkflowBuilder Do(Action<Context> action,
                               string? label = null)
        => Do(string.Empty, action, label);

    private IIfNode If(string id,
                               Func<Context, bool> predicate,
                               string? label = null) {
        var node = _nodeFactory.CreateIf(id, predicate);
        node.Label = label ?? node.Label;
        Connect(node);
        return node;
    }
    public IWorkflowBuilder If(string id,
                               Func<Context, bool> predicate,
                               Action<IWorkflowBuilder> setThen,
                               Action<IWorkflowBuilder>? setElse = null,
                               string? label = null) {
        var ifNode = If(id, predicate, label);
        var thenBuilder = new WorkflowBuilder(services);
        setThen(thenBuilder);
        ifNode.Then = thenBuilder._first;
        _nodes.AddRange(thenBuilder._nodes);

        if (setElse is null) return this;

        var elseBuilder = new WorkflowBuilder(services);
        setElse(elseBuilder);
        ifNode.Else = elseBuilder._first;
        _nodes.AddRange(elseBuilder._nodes);
        return this;
    }
    public IWorkflowBuilder If(Func<Context, bool> predicate,
                               Action<IWorkflowBuilder> setThen,
                               Action<IWorkflowBuilder>? setElse = null,
                               string? label = null)
        => If(string.Empty, predicate, setThen, setElse, label);

    private ICaseNode Case(string id,
                         Func<Context, string> select,
                         string? label = null) {
        var node = _nodeFactory.CreateCase(id, select);
        node.Label = label ?? node.Label;
        Connect(node);
        return node;
    }
    public IWorkflowBuilder Case(string id,
                                 Func<Context, string> select,
                                 Dictionary<string, Action<IWorkflowBuilder>> setCases,
                                 Action<IWorkflowBuilder>? setDefault = null,
                                 string? label = null) {
        var caseNode = Case(id, select, label);
        foreach ((var key, var buildChoice) in setCases) {
            var choiceBuilder = new WorkflowBuilder(services);
            buildChoice(choiceBuilder);
            _nodes.AddRange(choiceBuilder._nodes);
            caseNode.Choices[key] = choiceBuilder._first;
        }

        if (setDefault is null) return this;

        var defaultBuilder = new WorkflowBuilder(services);
        setDefault(defaultBuilder);
        _nodes.AddRange(defaultBuilder._nodes);
        caseNode.Choices[string.Empty] = defaultBuilder._first;
        return this;
    }
    public IWorkflowBuilder Case(Func<Context, string> select,
                                 Dictionary<string, Action<IWorkflowBuilder>> setCases,
                                 Action<IWorkflowBuilder>? setDefault = null,
                                 string? label = null)
        => Case(string.Empty, select, setCases, setDefault, label);

    public IWorkflowBuilder JumpTo(string targetNodeId,
                                   string? label = null) {
        var node = _nodeFactory.CreateJump(string.Empty, targetNodeId);
        node.Label = label ?? node.Label;
        Connect(node);
        return this;
    }

    public IWorkflowBuilder Exit(string id,
                                 int exitCode = 0,
                                 string? label = null) {
        var node = _nodeFactory.CreateExit(id, exitCode);
        node.Label = label ?? node.Label;
        Connect(node);
        return this;
    }
    public IWorkflowBuilder Exit(int exitCode = 0,
                                 string? label = null)
        => Exit(string.Empty, exitCode, label);

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
