namespace DotNetToolbox.Graph.Builders;

public sealed class WorkflowBuilder(IServiceProvider services, string? id = null)
    : IWorkflowBuilder {
    private readonly IServiceProvider _services = services;
    private readonly string _id = id ?? GuidProvider.Default.Create().ToString()!;
    private readonly INodeFactory _nodeFactory = services.GetRequiredService<INodeFactory>();

    internal List<INode> Nodes { get; } = [];

    public Result<INode?> Build()
        => ConnectNodes();

    public IWorkflowBuilder AddNode(INode node, Token? token = null) {
        Nodes.Add(IsNotNull(node));
        return this;
    }

    public IWorkflowBuilder Do<TAction>(string? ìd = null,
                                        string? label = null,
                                        Token? token = null)
        where TAction : ActionNode<TAction> {
        var node = _nodeFactory.Create<TAction>(ìd ?? string.Empty);
        node.Label = label ?? node.Label;
        Nodes.Add(node);
        return this;
    }

    public IWorkflowBuilder Do(string ìd,
                               Action<Context> action,
                               string? label = null,
                               Token? token = null) {
        var node = _nodeFactory.CreateAction(ìd, action);
        node.Label = label ?? node.Label;
        node.Token = token;
        Nodes.Add(node);
        return this;
    }
    public IWorkflowBuilder Do(Action<Context> action,
                               string? label = null,
                               Token? token = null)
        => Do(string.Empty, action, label, token);

    public IWorkflowBuilder If(string id,
                               Func<Context, bool> predicate,
                               string? label = null,
                               Token? token = null) {
        var node = _nodeFactory.CreateIf(id, predicate);
        node.Label = label ?? node.Label;
        node.Token = token;
        Nodes.Add(node);
        return this;
    }
    public IWorkflowBuilder If(string id,
                               Func<Context, bool> predicate,
                               Action<IWorkflowBuilder> setThen,
                               Action<IWorkflowBuilder>? setElse = null,
                               string? label = null,
                               Token? token = null) {
        If(id, predicate, label, token);
        setThen(this);
        setElse?.Invoke(this);
        return this;
    }
    public IWorkflowBuilder If(Func<Context, bool> predicate,
                               Action<IWorkflowBuilder> setThen,
                               Action<IWorkflowBuilder>? setElse = null,
                               string? label = null,
                               Token? token = null)
        => If(string.Empty, predicate, setThen, setElse, label, token);

    public IWorkflowBuilder Case(string id,
                                 Func<Context, string> select,
                                 string? label = null,
                                 Token? token = null) {
        var node = _nodeFactory.CreateCase(id, select);
        node.Label = label ?? node.Label;
        node.Token = token;
        Nodes.Add(node);
        return this;
    }
    public IWorkflowBuilder Case(string id,
                                 Func<Context, string> select,
                                 Dictionary<string, Action<IWorkflowBuilder>> setCases,
                                 Action<IWorkflowBuilder>? setDefault = null,
                                 string? label = null,
                                 Token? token = null) {
        Case(id, select, label, token);
        foreach (var (key, buildChoice) in setCases)
            buildChoice(this);
        setDefault?.Invoke(this);
        return this;
    }
    public IWorkflowBuilder Case(Func<Context, string> select,
                                 Dictionary<string, Action<IWorkflowBuilder>> setCases,
                                 Action<IWorkflowBuilder>? setDefault = null,
                                 string? label = null,
                                 Token? token = null)
        => Case(string.Empty, select, setCases, setDefault, label, token);

    public IWorkflowBuilder JumpTo(string id,
                                   string targetNodeId,
                                   string? label = null,
                                   Token? token = null) {
        var node = _nodeFactory.CreateJump(id, targetNodeId);
        node.Label = label ?? node.Label;
        node.Token = token;
        Nodes.Add(node);
        return this;
    }
    public IWorkflowBuilder JumpTo(string targetNodeId,
                                   string? label = null,
                                   Token? token = null)
        => JumpTo(string.Empty, targetNodeId, label, token);

    public IWorkflowBuilder Exit(string id,
                                 int exitCode = 0,
                                 string? label = null,
                                 Token? token = null) {
        var node = _nodeFactory.CreateExit(id, exitCode);
        node.Label = label ?? node.Label;
        node.Token = token;
        Nodes.Add(node);
        return this;
    }
    public IWorkflowBuilder Exit(int exitCode = 0,
                                 string? label = null,
                                 Token? token = null)
        => Exit(string.Empty, exitCode, label, token);

    private Result<INode?> ConnectNodes() {
        var current = Nodes.FirstOrDefault();
        var result = Success<INode?>(current);
        foreach (var node in Nodes.Skip(1)) {
            result += current?.ConnectTo(node) ?? Success();
            current = node;
        }

        //return ConnectJumps(result);
        return result;
    }

    //private Result ConnectJumps() {
    //    var result = Success();
    //    foreach (var jumpNode in Nodes.OfType<IJumpNode>()) {
    //        var targetNodeTag = Nodes.Find(n => n.Id == jumpNode.TargetTag);
    //        if (targetNodeTag is null)
    //            result += new ValidationError($"Jump target '{jumpNode.TargetTag}' not found.", jumpNode.Token?.ToSource());
    //        else jumpNode.ConnectTo(targetNodeTag);
    //    }
    //    return result;
    //}

    //public IElseNodeBuilder Then(Action<IWorkflowBuilder> setPath) {
    //    ((IfNode)_current!).Then = BuildBlock(setPath);
    //    return this;
    //}

    //public INodeBuilder Else(Action<IWorkflowBuilder> setPath) {
    //    ((IfNode)_current!).Else = BuildBlock(setPath);
    //    return this;
    //}

    //public ICaseIsNodeBuilder Is(string key, Action<IWorkflowBuilder> setPath) {
    //    ((CaseNode)_current!).Choices[IsNotNullOrWhiteSpace(key)] = BuildBlock(setPath);
    //    return this;
    //}

    //public INodeBuilder Otherwise(Action<IWorkflowBuilder> setPath) {
    //    ((CaseNode)_current!).Choices[string.Empty] = BuildBlock(setPath);
    //    return this;
    //}

    //private INode? BuildBlock(Action<IWorkflowBuilder> setPath) {
    //    var builder = new WorkflowBuilder(_services, _id);
    //    setPath(builder);
    //    Nodes.AddRange(builder.Nodes);
    //    return builder.Nodes.FirstOrDefault();
    //}
}
