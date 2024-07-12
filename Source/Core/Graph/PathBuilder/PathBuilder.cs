namespace DotNetToolbox.Graph.PathBuilder;

internal class PathBuilder
    : IPathBuilder, IThenBuilder, IElseBuilder {
    private INode? _current;

    private PathBuilder(INode? node = null) {
        _current = node;
    }

    public static IPathBuilder Start => new PathBuilder();

    public IThenBuilder If(Func<Map, bool> predicate) {
        _current = new IfNode(Guid.NewGuid().ToString(), IsNotNull(predicate));
        return this;
    }

    public IPathBuilder Do(Action<Map> execute) => throw new NotImplementedException();

    public IElseBuilder Then(INode node) {
        if (_current is not IfNode ifNode)
            throw new InvalidOperationException("Invalid node.");
        ifNode.SetTruePath(node);
        return this;
    }

    public IElseBuilder Then(Func<IPathBuilder, INode> configure)
        => Then(configure(new PathBuilder()));

    public IPathBuilder Else(INode node) {
        if (_current is not IfNode ifNode)
            throw new InvalidOperationException("Invalid node.");
        ifNode.SetFalsePath(node);
        return this;
    }

    public IPathBuilder Else(Func<IPathBuilder, INode> configure)
        => Else(configure(new PathBuilder()));

    public IThenBuilder ElseIf(Func<Map, bool> predicate) {
        _current = new IfNode(Guid.NewGuid().ToString(), IsNotNull(predicate));
        return this;
    }

    public IComparerBuilder<TKey> When<TKey>(Func<Map, TKey> select)
        => throw new NotImplementedException();

    public IPathBuilder Do(Func<Map, INode> execute)
        => throw new NotImplementedException();

    public INode End
        => throw new NotImplementedException();
}
