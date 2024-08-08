
namespace DotNetToolbox.Graph;

public interface IWorkflow {
    string Id { get; }
    Context Context { get; }
    INode StartNode { get; }

    Result Validate();
    Task Run(CancellationToken ct = default);
}
