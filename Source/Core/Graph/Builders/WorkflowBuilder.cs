namespace DotNetToolbox.Graph.Builders;

public sealed class WorkflowBuilder : IWorkflowBuilder,
      IIfNodeBuilder,
      IElseNodeBuilder,
      ICaseNodeBuilder,
      ICaseIsNodeBuilder {
    private readonly IServiceProvider _services;
    private readonly string _id;
    private readonly NodeFactory _nodeFactory;
    private readonly List<INode> _nodes = [];

    private INode? _first;
    private INode? _current;
    private Result _result = Success();

    public WorkflowBuilder(IServiceProvider services, string? id = null, INode? parent = null) {
        _services = services;
        _id = id ?? GuidProvider.Default.Create().ToString()!;
        _nodeFactory = (NodeFactory)services.GetRequiredService<INodeFactory>();
        _current = _first = parent;
    }

    internal Result<INode?> BuildBlock()
        => Success<INode?>(_first) + _result;

    public Result<INode?> Build()
        => BuildBlock() + ConnectJumps();

    internal IWorkflowBuilder Do(Token? token,
                                 string tag,
                                 Action<Context> action,
                                 string? label) {
        var node = _nodeFactory.CreateAction(tag, action, label);
        node.Token = token;
        _result += ConnectNode(node);
        return this;
    }

    public IWorkflowBuilder Do(string tag,
                               Action<Context> action,
                               string? label = null)
        => Do(null, tag, action, label);

    public IWorkflowBuilder Do(Action<Context> action, 
                               string? label = null)
        => Do(null, string.Empty, action, label);

    public IWorkflowBuilder Do<TAction>(string? tag = null,
                                        string? label = null)
        where TAction : ActionNode<TAction> {
        var node = _nodeFactory.Create<TAction>(tag ?? string.Empty, label);
        _result += ConnectNode(node);
        return this;
    }

    internal IWorkflowBuilder If(Token? token,
                                 string tag,
                                 Func<Context, bool> predicate,
                                 Action<IIfNodeBuilder> buildBranches,
                                 string? label = null) {
        var node = _nodeFactory.CreateIf(tag, predicate, null!, null, label);
        node.Token = token;
        var builder = new WorkflowBuilder(_services, _id, node);
        buildBranches(builder);
        _nodes.AddRange(builder._nodes);
        return this;
    }

    public IWorkflowBuilder If(string tag,
                               Func<Context, bool> predicate,
                               Action<IIfNodeBuilder> buildBranches,
                               string? label = null)
        => If(null, tag, predicate, buildBranches, label);

    public IWorkflowBuilder If(Func<Context, bool> predicate,
                               Action<IIfNodeBuilder> buildBranches,
                               string? label = null)
        => If(null, string.Empty, predicate, buildBranches, label);

    internal IWorkflowBuilder Case(Token? token,
                                   string tag,
                                   Func<Context, string> select,
                                   Action<ICaseNodeBuilder> buildChoices,
                                   string? label = null) {
        var node = _nodeFactory.CreateCase(tag, select, null!, null, label);
        node.Token = token;
        var builder = new WorkflowBuilder(_services, _id, node);
        buildBranches(builder);
        _result += ConnectNode(node);
        _nodes.AddRange(builder._nodes);
        return this;
    }

    public IWorkflowBuilder Case(string tag,
                                 Func<Context, string> select,
                                 Action<ICaseNodeBuilder> buildChoices,
                                 string? label = null) {
        var node = new CaseNode(tag, select);
        var builder = new WorkflowBuilder(_services, _id, node);
        buildChoices(builder);
        _result += ConnectNode(node);
        _nodes.AddRange(builder._nodes);
        return this;
    }

    public IWorkflowBuilder Case(Func<Context, string> select,
                                 Action<ICaseNodeBuilder> buildChoices,
                                 string? label = null)
        => Case(null!, select, buildChoices, label);

    internal IWorkflowBuilder Exit(Token token,
                                   string tag,
                                   int exitCode = 0,
                                   string? label = null) {
        var node = _nodeFactory.CreateExit(tag, exitCode, label);
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
        var node = _nodeFactory.CreateJump(_sequence.Next.ToString(), IsNotNullOrWhiteSpace(targetTag), tag, label);
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
            var targetNode = _nodes.Find(n => n.Id == jumpNode.TargetTag);
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
        var result = builder.BuildBlock();
        _result += result;
        _nodes.AddRange(builder._nodes);
        return result.Value;
    }
}
