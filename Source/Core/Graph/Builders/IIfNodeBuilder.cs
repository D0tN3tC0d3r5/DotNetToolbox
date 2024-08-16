namespace DotNetToolbox.Graph.Builders;

public interface IIfNodeBuilder {
    IElseNodeBuilder IsTrue(Action<IWorkflowBuilder> setPath);
}
