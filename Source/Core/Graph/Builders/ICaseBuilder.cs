namespace DotNetToolbox.Graph.Builders;

public interface ICaseBuilder : IWorkflowBuilder {
    IOtherwiseBuilder Is(string key, Action<IWorkflowBuilder> setCase);
}
