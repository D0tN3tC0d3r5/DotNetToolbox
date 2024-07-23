namespace DotNetToolbox.Graph;

public class BranchesBuilder(IBranchingNode owner, INodeFactory? factory = null) {

    public BranchesBuilder Case(string key, Action<WorkflowBuilder> setPath) {
        var branchBuilder = new WorkflowBuilder(factory);
        setPath(branchBuilder);
        owner.Branches[key] = branchBuilder.Start;
        return this;
    }
}
