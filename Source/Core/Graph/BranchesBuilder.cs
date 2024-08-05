namespace DotNetToolbox.Graph;

public class BranchesBuilder(WorkflowBuilder builder, IBranchingNode owner, IServiceProvider services) {
    public BranchesBuilder Is(string key, Action<WorkflowBuilder> setPath) {
        var branchBuilder = new WorkflowBuilder(builder.Id, builder.Nodes, services);
        setPath(branchBuilder);
        owner.Choices[key] = branchBuilder.Start;
        return this;
    }
}
