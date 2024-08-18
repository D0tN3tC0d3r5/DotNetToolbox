namespace DotNetToolbox.Graph.Factories;

internal sealed class NodeFactory(IServiceProvider services)
    : INodeFactory {
    //private readonly string _builderId = id ?? GuidProvider.Default.Create().ToString()!;
    private readonly INodeSequence? _sequence = services.GetService<INodeSequence>() ?? NodeSequence.Shared;

    public TNode Create<TNode>(string? id = null)
        where TNode : Node<TNode>
        => Node.Create<TNode>(id ?? string.Empty, _sequence);

    public IActionNode CreateAction(string id,
                                    Action<Context> action) {
        var policy = services.GetService<IPolicy>() ?? Policy.Default;
        return new ActionNode(id, action, _sequence, policy);
    }
    public IActionNode CreateAction(Action<Context> action) {
        var policy = services.GetService<IPolicy>() ?? Policy.Default;
        return new ActionNode(action, _sequence, policy);
    }

    public IIfNode CreateIf(string id,
                            Func<Context, bool> predicate)
        => new IfNode(id, predicate, _sequence);
    public IIfNode CreateIf(Func<Context, bool> predicate)
        => new IfNode(predicate, _sequence);
    public IIfNode CreateIf(string id,
                            Func<Context, bool> predicate,
                            INode truePath,
                            INode? falsePath = null)
        => new IfNode(id, predicate, _sequence) {
            Then = truePath,
            Else = falsePath,
        };
    public IIfNode CreateIf(Func<Context, bool> predicate,
                            INode truePath,
                            INode? falsePath = null)
        => new IfNode(predicate, _sequence) {
            Then = truePath,
            Else = falsePath,
        };

    public ICaseNode CreateCase(string id,
                                Func<Context, string> selectPath)
        => new CaseNode(id, selectPath, _sequence);

    public ICaseNode CreateCase(Func<Context, string> selectPath)
        => new CaseNode(selectPath, _sequence);

    public ICaseNode CreateCase(string id,
                                Func<Context, string> selectPath,
                                Dictionary<string, INode?> choices,
                                INode? otherwise = null) {
        var node = new CaseNode(id, selectPath, _sequence);
        foreach (var choice in choices) node.Choices.Add(IsNotNullOrEmpty(choice.Key), choice.Value);
        node.Choices.Add(string.Empty, otherwise);
        return node;
    }
    public ICaseNode CreateCase(Func<Context, string> selectPath,
                                Dictionary<string, INode?> choices,
                                INode? otherwise = null) {
        var node = new CaseNode(selectPath, _sequence);
        foreach (var choice in choices) node.Choices.Add(IsNotNullOrEmpty(choice.Key), choice.Value);
        node.Choices.Add(string.Empty, otherwise);
        return node;
    }

    public IJumpNode CreateJump(string id,
                                string targetTag)
        => new JumpNode(id, targetTag, _sequence);
    public IJumpNode CreateJump(string targetTag)
        => new JumpNode(targetTag, _sequence);

    public IExitNode CreateExit(string id,
                                int exitCode = 0)
        => new ExitNode(id, exitCode, _sequence);
    public IExitNode CreateExit(int exitCode = 0)
        => new ExitNode(exitCode, _sequence);
}
