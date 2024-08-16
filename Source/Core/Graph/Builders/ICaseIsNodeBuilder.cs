namespace DotNetToolbox.Graph.Builders;

public interface ICaseIsNodeBuilder {
    ICaseIsNodeBuilder Is(string key, Action<IWorkflowBuilder> setPath);
    INodeBuilder Otherwise(Action<IWorkflowBuilder> setPath);
}
