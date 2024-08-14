namespace DotNetToolbox.Graph.Builders;

public interface ICaseOptionNodeBuilder
    : INodeBuilder<ICaseNode> {
    ICaseOptionNodeBuilder Is(string key, Action<IWorkflowBuilder> setPath);
    INodeBuilder<ICaseNode> Otherwise(Action<IWorkflowBuilder> setPath);
}
