namespace DotNetToolbox.Graph;

public class WorkflowEventArgs(Workflow workflow)
    : EventArgs {
    public Workflow Workflow { get; } = workflow;
}
