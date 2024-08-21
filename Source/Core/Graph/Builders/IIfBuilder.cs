namespace DotNetToolbox.Graph.Builders;

public interface IIfBuilder {
    IElseBuilder Then(Action<IWorkflowBuilder> setThen);
}
