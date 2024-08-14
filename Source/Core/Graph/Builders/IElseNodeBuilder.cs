namespace DotNetToolbox.Graph.Builders;

public interface IElseNodeBuilder
    : INodeBuilder<IIfNode> {
    INodeBuilder<IIfNode> IsFalse(Action<IWorkflowBuilder> setPath);
}
