namespace DotNetToolbox.Graph;

public class Workflow(string id,
                      INode start,
                      Context context,
                      IDateTimeProvider? dateTime = null,
                      IGuidProvider? guid = null,
                      ILoggerFactory? loggerFactory = null)
    : IWorkflow {
    public Workflow(INode start,
                    Context context,
                    IDateTimeProvider? dateTime = null,
                    ILoggerFactory? loggerFactory = null,
                    IGuidProvider? guid = null)
        : this((guid ?? GuidProvider.Default).AsSortable.Create().ToString(),
               start,
               context,
               dateTime,
               guid,
               loggerFactory) {
    }

    public string Id { get; } = id;
    public INode StartNode { get; } = IsNotNull(start);
    public Context Context { get; } = IsNotNull(context);

    public Result Validate() => ValidateNode(StartNode);

    private static Result ValidateNode(INode? node, ISet<INode>? visited = null) {
        if (node is null) return Success();
        visited ??= new HashSet<INode>();
        var result = !visited.Add(node)
                         ? Success()
                         : node.Validate(visited);
        switch (node) {
            case IActionNode n:
                result += ValidateNode(n.Next, visited);
                break;
            case IIfNode n:
                result += ValidateNode(n.IsTrue, visited);
                result += ValidateNode(n.IsFalse, visited);
                break;
            case ICaseNode n:
                foreach ((_, var branch) in n.Choices)
                    result += ValidateNode(branch, visited);
                break;
        }
        return result;
    }

    public Task Run(CancellationToken ct = default) {
        var runner = new Runner(this, dateTime, guid, loggerFactory);
        return runner.Run(ct);
    }
}
