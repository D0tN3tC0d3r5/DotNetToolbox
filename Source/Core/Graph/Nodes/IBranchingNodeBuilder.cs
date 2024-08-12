namespace DotNetToolbox.Graph.Nodes;

public interface IBranchingNodeBuilder {
    IBranchingNodeBuilder Is(string key, Action<WorkflowBuilder> setPath);
    void Otherwise(Action<WorkflowBuilder> setPath);
}
