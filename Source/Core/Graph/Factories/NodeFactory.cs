namespace DotNetToolbox.Graph.Factories;

internal sealed class NodeFactory(IServiceProvider services)
    : INodeFactory {
    public TNode Create<TNode>(string? id = null)
        where TNode : Node<TNode>
        => Node.Create<TNode>(services, id ?? string.Empty);

    public IActionNode CreateAction(string id, Func<Context, CancellationToken, Task> action)
        => new ActionNode(action, services) { Tag = id };

    public IActionNode CreateAction(Func<Context, CancellationToken, Task> action)
        => new ActionNode(action, services);

    public IActionNode CreateAction(string id, Action<Context> action)
        => new ActionNode(action, services) { Tag = id };

    public IActionNode CreateAction(Action<Context> action)
        => new ActionNode(action, services);

    public IIfNode CreateIf(string id, Func<Context, CancellationToken, Task<bool>> predicate)
        => new IfNode(predicate, services) { Tag = id };
    public IIfNode CreateIf(Func<Context, CancellationToken, Task<bool>> predicate)
        => new IfNode(predicate, services);
    public IIfNode CreateIf(string id,
                            Func<Context, CancellationToken, Task<bool>> predicate,
                            INode truePath,
                            INode? falsePath = null)
        => new IfNode(predicate, services) {
            Tag = id,
            Then = truePath,
            Else = falsePath,
        };
    public IIfNode CreateIf(Func<Context, CancellationToken, Task<bool>> predicate,
                            INode truePath,
                            INode? falsePath = null)
        => new IfNode(predicate, services) {
            Then = truePath,
            Else = falsePath,
        };
    public IIfNode CreateIf(string id, Func<Context, bool> predicate)
        => new IfNode(predicate, services) { Tag = id };
    public IIfNode CreateIf(Func<Context, bool> predicate)
        => new IfNode(predicate, services);
    public IIfNode CreateIf(string id,
                            Func<Context, bool> predicate,
                            INode truePath,
                            INode? falsePath = null)
        => new IfNode(predicate, services) {
            Tag = id,
            Then = truePath,
            Else = falsePath,
        };
    public IIfNode CreateIf(Func<Context, bool> predicate,
                            INode truePath,
                            INode? falsePath = null)
        => new IfNode(predicate, services) {
            Then = truePath,
            Else = falsePath,
        };

    public ICaseNode CreateCase(string id,
                                Func<Context, CancellationToken, Task<string>> selectPath)
        => new CaseNode(selectPath, services) { Tag = id };

    public ICaseNode CreateCase(Func<Context, CancellationToken, Task<string>> selectPath)
        => new CaseNode(selectPath, services);

    public ICaseNode CreateCase(string id,
                                Func<Context, CancellationToken, Task<string>> selectPath,
                                Dictionary<string, INode?> choices,
                                INode? otherwise = null) {
        var node = new CaseNode(selectPath, services) { Tag = id };
        foreach (var choice in choices) node.Choices.Add(IsNotNullOrEmpty(choice.Key), choice.Value);
        node.Choices.Add(string.Empty, otherwise);
        return node;
    }
    public ICaseNode CreateCase(Func<Context, CancellationToken, Task<string>> selectPath,
                                Dictionary<string, INode?> choices,
                                INode? otherwise = null) {
        var node = new CaseNode(selectPath, services);
        foreach (var choice in choices) node.Choices.Add(IsNotNullOrEmpty(choice.Key), choice.Value);
        node.Choices.Add(string.Empty, otherwise);
        return node;
    }
    public ICaseNode CreateCase(string id,
                                Func<Context, string> selectPath)
        => new CaseNode(selectPath, services) { Tag = id };

    public ICaseNode CreateCase(Func<Context, string> selectPath)
        => new CaseNode(selectPath, services);

    public ICaseNode CreateCase(string id,
                                Func<Context, string> selectPath,
                                Dictionary<string, INode?> choices,
                                INode? otherwise = null) {
        var node = new CaseNode(selectPath, services) { Tag = id };
        foreach (var choice in choices) node.Choices.Add(IsNotNullOrEmpty(choice.Key), choice.Value);
        node.Choices.Add(string.Empty, otherwise);
        return node;
    }
    public ICaseNode CreateCase(Func<Context, string> selectPath,
                                Dictionary<string, INode?> choices,
                                INode? otherwise = null) {
        var node = new CaseNode(selectPath, services);
        foreach (var choice in choices) node.Choices.Add(IsNotNullOrEmpty(choice.Key), choice.Value);
        node.Choices.Add(string.Empty, otherwise);
        return node;
    }

    public IJumpNode CreateJump(string targetTag)
        => new JumpNode(targetTag, services);

    public IExitNode CreateExit(string id,
                                int exitCode = 0)
        => new ExitNode(id, exitCode, services);
    public IExitNode CreateExit(int exitCode = 0)
        => new ExitNode(exitCode, services);
}
