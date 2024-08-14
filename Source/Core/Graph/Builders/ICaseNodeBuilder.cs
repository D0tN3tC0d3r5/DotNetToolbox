namespace DotNetToolbox.Graph.Builders;

public interface ICaseNodeBuilder
    : INodeBuilder<ICaseNode> {
    ICaseOptionNodeBuilder Is(string key, Action<IWorkflowBuilder> setPath);
}
