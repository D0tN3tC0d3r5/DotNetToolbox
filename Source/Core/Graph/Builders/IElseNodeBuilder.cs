namespace DotNetToolbox.Graph.Builders;

public interface IElseNodeBuilder {
    INodeBuilder IsFalse(Action<IWorkflowBuilder> setPath);
}
