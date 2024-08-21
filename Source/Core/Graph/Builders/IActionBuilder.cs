namespace DotNetToolbox.Graph.Builders;

public interface IActionBuilder : IWorkflowBuilder {
    IWorkflowBuilder WithRetry(IRetryPolicy retry);
}
