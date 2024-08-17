namespace DotNetToolbox.Graph.Factories;

internal sealed class NodeFactory(IServiceProvider services)
    : INodeFactory {
    //private readonly string _builderId = id ?? GuidProvider.Default.Create().ToString()!;
    private readonly INodeSequence? _sequence = services.GetService<INodeSequence>() ?? NodeSequence.Shared;

    public TNode Create<TNode>(string id, string? label = null)
        where TNode : Node<TNode> {
        var node = Node.Create<TNode>(id, _sequence);
        node.Label = label ?? string.Empty;
        return node;
    }
    public TNode Create<TNode>(string? label = null)
        where TNode : Node<TNode>
        => Create<TNode>(string.Empty, label);

    public INode CreateAction(string id,
                              Action<Context> action,
                              string? label = null) {
        var policy = services.GetService<IPolicy>() ?? Policy.Default;
        return new ActionNode(id, action, _sequence, policy) {
            Label = label ?? string.Empty,
        };
    }
    public INode CreateAction(Action<Context> action,
                              string? label = null) {
        var policy = services.GetService<IPolicy>() ?? Policy.Default;
        return new ActionNode(action, _sequence, policy) {
            Label = label ?? string.Empty,
        };
    }

    public INode CreateIf(string id,
                            Func<Context, bool> predicate,
                            INode truePath,
                            INode? falsePath = null,
                            string? label = null)
        => new IfNode(id, predicate, _sequence) {
            Label = label ?? string.Empty,
            IsTrue = truePath,
            IsFalse = falsePath,
        };
    public INode CreateIf(Func<Context, bool> predicate,
                          INode truePath,
                          INode? falsePath = null,
                          string? label = null)
        => new IfNode(predicate, _sequence) {
            Label = label ?? string.Empty,
            IsTrue = truePath,
            IsFalse = falsePath,
        };

    public INode CreateCase(string id,
                            Func<Context, string> selectPath,
                            Dictionary<string, INode?> choices,
                            INode? otherwise = null,
                            string? label = null) {
        var node = new CaseNode(id, selectPath, _sequence) {
            Label = label ?? string.Empty,
        };
        foreach (var choice in choices) node.Choices.Add(IsNotNullOrEmpty(choice.Key), choice.Value);
        node.Choices.Add(string.Empty, otherwise);
        return node;
    }
    public INode CreateCase(Func<Context, string> selectPath,
                            Dictionary<string, INode?> choices,
                            INode? otherwise = null,
                            string? label = null) {
        var node = new CaseNode(selectPath, _sequence) {
            Label = label ?? string.Empty,
        };
        foreach (var choice in choices) node.Choices.Add(IsNotNullOrEmpty(choice.Key), choice.Value);
        node.Choices.Add(string.Empty, otherwise);
        return node;
    }
    public INode CreateJump(string id,
                            string targetTag,
                            string? label = null)
        => new JumpNode(id, targetTag, _sequence) {
            Label = label ?? string.Empty,
        };
    public INode CreateJump(string targetTag,
                            string? label = null)
        => new JumpNode(targetTag, _sequence) {
            Label = label ?? string.Empty,
        };

    public INode CreateExit(string id,
                            int exitCode = 0,
                            string? label = null)
        => new ExitNode(id, exitCode, _sequence) {
            Label = label ?? string.Empty,
        };
    public INode CreateExit(int exitCode = 0,
                            string? label = null)
        => new ExitNode(exitCode, _sequence) {
            Label = label ?? string.Empty,
        };
}
