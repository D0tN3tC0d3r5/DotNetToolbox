﻿namespace DotNetToolbox.Graph.Factories;

internal sealed class NodeFactory(IServiceProvider services)
    : INodeFactory {
    public TNode Create<TNode>(string? tag = null, params object[] args)
        where TNode : Node<TNode>
        => Node.Create<TNode>(services, [tag ?? string.Empty, .. args]);

    public IActionNode CreateAction(string tag, Func<Context, CancellationToken, Task> action)
        => new ActionNode(action, services) { Tag = tag };

    public IActionNode CreateAction(Func<Context, CancellationToken, Task> action)
        => new ActionNode(action, services);

    public IActionNode CreateAction(string tag, Action<Context> action)
        => new ActionNode(action, services) { Tag = tag };

    public IActionNode CreateAction(Action<Context> action)
        => new ActionNode(action, services);

    public IIfNode CreateIf(string tag, Func<Context, CancellationToken, Task<bool>> predicate)
        => new IfNode(predicate, services) { Tag = tag };
    public IIfNode CreateIf(Func<Context, CancellationToken, Task<bool>> predicate)
        => new IfNode(predicate, services);
    public IIfNode CreateIf(string tag,
                            Func<Context, CancellationToken, Task<bool>> predicate,
                            INode truePath,
                            INode? falsePath = null)
        => new IfNode(predicate, services) {
            Tag = tag,
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
    public IIfNode CreateIf(string tag, Func<Context, bool> predicate)
        => new IfNode(predicate, services) { Tag = tag };
    public IIfNode CreateIf(Func<Context, bool> predicate)
        => new IfNode(predicate, services);
    public IIfNode CreateIf(string tag,
                            Func<Context, bool> predicate,
                            INode truePath,
                            INode? falsePath = null)
        => new IfNode(predicate, services) {
            Tag = tag,
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

    public ICaseNode CreateCase(string tag,
                                Func<Context, CancellationToken, Task<string>> selectPath)
        => new CaseNode(selectPath, services) { Tag = tag };

    public ICaseNode CreateCase(Func<Context, CancellationToken, Task<string>> selectPath)
        => new CaseNode(selectPath, services);

    public ICaseNode CreateCase(string tag,
                                Func<Context, CancellationToken, Task<string>> selectPath,
                                Dictionary<string, INode?> choices,
                                INode? otherwise = null) {
        var node = new CaseNode(selectPath, services) { Tag = tag };
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
    public ICaseNode CreateCase(string tag,
                                Func<Context, string> selectPath)
        => new CaseNode(selectPath, services) { Tag = tag };

    public ICaseNode CreateCase(Func<Context, string> selectPath)
        => new CaseNode(selectPath, services);

    public ICaseNode CreateCase(string tag,
                                Func<Context, string> selectPath,
                                Dictionary<string, INode?> choices,
                                INode? otherwise = null) {
        var node = new CaseNode(selectPath, services) { Tag = tag };
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

    public IExitNode CreateExit(string tag,
                                int exitCode = 0)
        => new ExitNode(tag, exitCode, services);
    public IExitNode CreateExit(int exitCode = 0)
        => new ExitNode(exitCode, services);
}
