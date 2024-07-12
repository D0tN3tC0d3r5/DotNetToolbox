namespace DotNetToolbox.Graph.PathBuilder;

public static class NodeBuilder {
    public static IIfBuilder If(Func<Map, bool> predicate)
        => PathBuilder.From(IfNode.Create(predicate));
}

public class PathBuilder
    : IPathBuilder, IIfBuilder, IThenBuilder, IEndBuilder {
    private INode? _current;

    private PathBuilder(INode? node = null) {
        _current = node;
    }

    public static PathBuilder From(INode node) => new(node);
    public static PathBuilder Create() => new();

    public IIfBuilder If(Func<Map, bool> predicate) {
        _current = new IfNode(Guid.NewGuid().ToString(), IsNotNull(predicate));
        return this;
    }

    public IPathBuilder Do(Action<Map> execute) => throw new NotImplementedException();

    public IThenBuilder Then(INode node) {
        if (_current is not IfNode ifNode)
            throw new InvalidOperationException("Invalid node.");
        ifNode.SetTruePath(node);
        return this;
    }

    public IThenBuilder Then(Action<IPathBuilder> build) {
        if (_current is not IfNode ifNode)
            throw new InvalidOperationException("Invalid node.");
        var builder = Create();
        build(builder);
        var node = builder.Build();
        ifNode.SetTruePath(node);
        return this;
    }

    public IPathBuilder Else(INode node) {
        if (_current is not IfNode ifNode)
            throw new InvalidOperationException("Invalid node.");
        ifNode.SetFalsePath(node);
        return this;
    }

    public IPathBuilder Else(Action<IPathBuilder> build) {
        if (_current is not IfNode ifNode)
            throw new InvalidOperationException("Invalid node.");
        var builder = Create();
        build(builder);
        var node = builder.Build();
        ifNode.SetFalsePath(node);
        return this;
    }

    public IIfBuilder ElseIf(Func<Map, bool> predicate) {
        _current = new IfNode(Guid.NewGuid().ToString(), IsNotNull(predicate));
        return this;
    }

    public ISwitchBuilder<TKey> Select<TKey>(Func<Map, TKey> select) => throw new NotImplementedException();

    public IEndBuilder End() => throw new NotImplementedException();
    public INode Build() => throw new NotImplementedException();
}
