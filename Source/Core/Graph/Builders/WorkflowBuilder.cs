namespace DotNetToolbox.Graph.Builders;

public sealed class WorkflowBuilder : IWorkflowBuilder,
      IIfNodeBuilder,
      IElseNodeBuilder,
      ICaseNodeBuilder,
      ICaseIsNodeBuilder {
    private readonly IServiceProvider _services;
    private readonly string _id;
    private readonly NodeFactory _nodeFactory;
    private readonly IIdGenerator<uint> _idGenerator;
    private readonly List<INode> _nodes = [];

    private INode? _first;
    private INode? _current;
    private Result _result = Success();

    public WorkflowBuilder(IServiceProvider services, string? id = null, INode? parent = null) {
        _services = services;
        _id = id ?? GuidProvider.Default.Create().ToString()!;
        _nodeFactory = (NodeFactory)services.GetRequiredService<INodeFactory>();
        _idGenerator = services.GetKeyedService<IIdGenerator<uint>>(_id) ?? NodeIdGenerator.Keyed(_id);
        _current = _first = parent;
    }

    public Result<INode?> Build() {
        var result = Success<INode?>(_first);
        result += _result + ConnectJumps();
        return result;
    }

    internal IWorkflowBuilder Do(Token token,
                                 Action<Context> action,
                                 string? tag,
                                 string? label) {
        var node = _nodeFactory.CreateAction(_idGenerator.Consume(), action, tag, label);
        _result += ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder Do(string tag,
                               Action<Context> action,
                               string? label = null)
        => Do(null!, action, IsNotNullOrWhiteSpace(tag), label);

    public IWorkflowBuilder Do(Action<Context> action,
                               string? label = null)
        => Do(null!, action, null, label);

    public IWorkflowBuilder Do<TAction>(string? tag = null,
                                        string? label = null)
        where TAction : ActionNode<TAction> {
        var node = _nodeFactory.Create<TAction>(_idGenerator.Consume(), tag, label);
        _result += ConnectNode(node);
        return this;
    }

    internal IWorkflowBuilder If(Token token,
                                 Func<Context, bool> predicate,
                                 Action<IIfNodeBuilder> buildBranches,
                                 string? tag,
                                 string? label) {
        var result = _nodeFactory.GetCreateForkResult(_idGenerator.Consume(), predicate, buildBranches, tag, label);
        var node = result.Value!;
        node.Token = token;
        _result += result + ConnectNode(node);
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
        var result = _nodeFactory.GetCreateChoiceResult(_idGenerator.Consume(), select, buildChoices, tag, label);
        var node = result.Value!;
        node.Token = token;
        _result += result + ConnectNode(node);
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
        var node = _nodeFactory.CreateExit(_idGenerator.Consume(), exitCode, tag, label);
        node.Token = token;
        _result += ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder Exit(string? tag = null, int exitCode = 0, string? label = null)
        => Exit(null!, exitCode, tag, label);

    internal IWorkflowBuilder JumpTo(Token token,
                                     string targetTag,
                                     string? tag,
                                     string? label) {
        var node = _nodeFactory.CreateJump(_idGenerator.Consume(), IsNotNullOrWhiteSpace(targetTag), tag, label);
        node.Token = token;
        _result += ConnectNode(node);
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
        var result = _current is not null and not IJumpNode
            ? _current.ConnectTo(node)
            : Success();
        _current = node;
        _nodes.Add(node);
        return result;
    }

    private Result ConnectJumps() {
        var result = Success();
        foreach (var jumpNode in _nodes.OfType<IJumpNode>()) {
            var targetNode = _nodes.Find(n => n.Tag == jumpNode.TargetTag);
            if (targetNode is null)
                result += new ValidationError($"Jump target '{jumpNode.TargetTag}' not found.", jumpNode.Token?.ToSource());
            else jumpNode.ConnectTo(targetNode);
        }
        return result;
    }

    public IElseNodeBuilder IsTrue(Action<IWorkflowBuilder> setPath) {
        ((IfNode)_current!).IsTrue = buildBlock(setPath);
        return this;
    }

    public INodeBuilder IsFalse(Action<IWorkflowBuilder> setPath) {
        ((IfNode)_current!).IsFalse = buildBlock(setPath);
        return this;
    }

    public ICaseIsNodeBuilder Is(string key, Action<IWorkflowBuilder> setPath) {
        ((CaseNode)_current!).Choices[IsNotNullOrWhiteSpace(key)] = buildBlock(setPath);
        return this;
    }

    public INodeBuilder Otherwise(Action<IWorkflowBuilder> setPath) {
        ((CaseNode)_current!).Choices[string.Empty] = buildBlock(setPath);
        return this;
    }

    private INode? buildBlock(Action<IWorkflowBuilder> setPath) {
        var builder = new WorkflowBuilder(_services, _id);
        setPath(builder);
        var result = builder.Build();
        _result += result;
        _nodes.AddRange(builder._nodes);
        return result.Value;
    }
}
