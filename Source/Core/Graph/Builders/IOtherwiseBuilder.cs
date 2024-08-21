namespace DotNetToolbox.Graph.Builders;

public interface IOtherwiseBuilder : ICaseBuilder {
    IWorkflowBuilder Otherwise(Action<IWorkflowBuilder> setOtherwise);
}
