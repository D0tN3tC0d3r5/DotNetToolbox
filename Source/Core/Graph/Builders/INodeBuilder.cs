namespace DotNetToolbox.Graph.Builders;

public interface INodeBuilder {
    IWorkflowBuilder WithLabel(string label);
    INode Build();
}
