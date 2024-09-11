
namespace DotNetToolbox.Graph;

public interface IWorkflow {
    string Id { get; }
    Map Map { get; }
    INode StartNode { get; }

    Result Validate();
    Task Run(CancellationToken ct = default);
}
