namespace DotNetToolbox.Graph.Nodes;

public class BranchingNodeBuilder(IServiceProvider services)
    : IBranchingNodeBuilder {
    private readonly Dictionary<string, INode?> _choices = [];

    public IBranchingNodeBuilder Is(string key, Action<WorkflowBuilder> setPath) {
        var branchBuilder = new WorkflowBuilder(services);
        setPath(branchBuilder);
        _choices[IsNotNullOrWhiteSpace(key)] = branchBuilder.First;
        return this;
    }

    public void Otherwise(Action<WorkflowBuilder> setPath) {
        var branchBuilder = new WorkflowBuilder(services);
        setPath(branchBuilder);
        _choices[string.Empty] = branchBuilder.First;
    }

    public void Configure(IBranchingNode owner) {
        owner.Choices.Clear();
        foreach (var (key, value) in _choices)
            owner.Choices[key] = value;
    }
}
