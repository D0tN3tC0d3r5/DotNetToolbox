namespace DotNetToolbox.Graph;

public class BranchesBuilder(HashSet<INode?>? nodes, IBranchingNode owner) {

    public BranchesBuilder Case(string key, Action<WorkflowBuilder> setPath) {
        var branchBuilder = new WorkflowBuilder(nodes);
        setPath(branchBuilder);
        owner.Choices[key] = branchBuilder.Start;
        return this;
    }
}
