namespace DotNetToolbox.Graph.Builders;

public class CaseNodeBuilder(IServiceProvider services, ICaseNode parent, string nodeSequenceKey, Dictionary<string, INode> tagMap)
    : ICaseNodeBuilder,
      ICaseOptionNodeBuilder {
    private readonly Dictionary<string, INode?> _choices = [];
    private readonly ICaseNode _parent = IsNotNull(parent);

    public ICaseOptionNodeBuilder Is(string key, Action<IWorkflowBuilder> setPath) {
        var branchBuilder = new WorkflowBuilder(services, nodeSequenceKey, tagMap);
        setPath(branchBuilder);
        _choices[IsNotNullOrWhiteSpace(key)] = branchBuilder.BuildBlock();
        return this;
    }

    public INodeBuilder<ICaseNode> Otherwise(Action<IWorkflowBuilder> setPath) {
        var branchBuilder = new WorkflowBuilder(services, nodeSequenceKey, tagMap);
        setPath(branchBuilder);
        _choices[string.Empty] = branchBuilder.BuildBlock();
        return this;
    }

    public ICaseNode Build() {
        _parent.Choices.Clear();
        foreach ((var key, var value) in _choices)
            _parent.Choices[key] = value;
        return _parent;
    }
}
