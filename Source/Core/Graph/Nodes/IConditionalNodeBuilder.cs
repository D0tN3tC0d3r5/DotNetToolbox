namespace DotNetToolbox.Graph.Nodes;

public interface IConditionalNodeTruePathBuilder {
    IConditionalNodeFalsePathBuilder IsTrue(Action<WorkflowBuilder> setPath);
}

public interface IConditionalNodeFalsePathBuilder {
    void IsFalse(Action<WorkflowBuilder> setPath);
}
