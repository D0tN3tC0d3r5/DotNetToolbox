namespace DotNetToolbox.Graph.Nodes;

public class ConditionalNodeBuilder(WorkflowBuilder builder, IServiceProvider services)
    : IConditionalNodeTruePathBuilder, IConditionalNodeFalsePathBuilder {
    private INode? _trueNode;
    private INode? _falseNode;

    public IConditionalNodeFalsePathBuilder IsTrue(Action<WorkflowBuilder> setPath) {
        var trueBuilder = new WorkflowBuilder(services, builder.Id, builder.Nodes);
        setPath(trueBuilder);
        _trueNode = trueBuilder.First;
        return this;
    }

    public void IsFalse(Action<WorkflowBuilder> setPath) {
        var falseBuilder = new WorkflowBuilder(services, builder.Id, builder.Nodes);
        setPath(falseBuilder);
        _falseNode = falseBuilder.First;
    }

    public void Configure(IConditionalNode owner) {
        if (_trueNode == null)
            throw new InvalidOperationException("True branch is required.");
        owner.IsTrue = _trueNode;
        owner.IsFalse = _falseNode;
    }
}
