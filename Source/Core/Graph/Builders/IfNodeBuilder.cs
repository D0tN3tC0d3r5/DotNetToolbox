namespace DotNetToolbox.Graph.Builders;

public class IfNodeBuilder(IServiceProvider services, IIfNode parent)
    : IIfNodeBuilder,
      IElseNodeBuilder {
    private INode? _trueNode;
    private INode? _falseNode;
    private readonly IIfNode _parent = IsNotNull(parent);

    public IElseNodeBuilder IsTrue(Action<IWorkflowBuilder> setPath) {
        var trueBuilder = new WorkflowBuilder(services);
        setPath(trueBuilder);
        _trueNode = trueBuilder.Build();
        return this;
    }

    public INodeBuilder<IIfNode> IsFalse(Action<IWorkflowBuilder> setPath) {
        var falseBuilder = new WorkflowBuilder(services);
        setPath(falseBuilder);
        _falseNode = falseBuilder.Build();
        return this;
    }

    public IIfNode Build() {
        if (_trueNode == null)
            throw new InvalidOperationException("True branch is required.");
        _parent.IsTrue = _trueNode;
        _parent.IsFalse = _falseNode;
        return _parent;
    }
}
