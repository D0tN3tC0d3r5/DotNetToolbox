namespace DotNetToolbox.Graph.Builders;

public interface IIfNodeBuilder
    : INodeBuilder<IIfNode> {
    IElseNodeBuilder IsTrue(Action<IWorkflowBuilder> setPath);
}
