namespace DotNetToolbox.Graph.PathBuilder;

public interface IIfBuilder {
    IPathBuilder Then(INode node);
    IPathBuilder Then(Action<IPathBuilder> builder);
    IPathBuilder Else(INode node);
    IPathBuilder Else(Action<IPathBuilder> builder);
}

public class IfBuilder(IfThenElseNode caller)
    : IIfBuilder {

    public IPathBuilder Then(INode node) {
        caller.SetTruePath(node);
        return this;
    }

    public IPathBuilder Then(Action<IPathBuilder> builder) {
        throw new NotImplementedException();
    }

    public IPathBuilder Else(INode node) => throw new NotImplementedException();

    public IPathBuilder Else(Action<IPathBuilder> builder) => throw new NotImplementedException();
}
