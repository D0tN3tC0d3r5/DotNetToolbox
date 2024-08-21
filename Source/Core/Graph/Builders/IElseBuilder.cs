namespace DotNetToolbox.Graph.Builders;

public interface IElseBuilder : IWorkflowBuilder {
    IWorkflowBuilder Else(Action<IWorkflowBuilder> setElse);
}
