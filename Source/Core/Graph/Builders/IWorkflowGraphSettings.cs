namespace DotNetToolbox.Graph.Builders;

public interface IWorkflowGraphSettings {
    IWorkflowGraphSettings Format(GraphFormat format);
    IWorkflowGraphSettings Direction(GraphDirection direction);
}
