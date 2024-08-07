namespace DotNetToolbox.Graph;

public class Workflow(INode start, Context context, IGuidProvider? guid = null) {
    public string Id { get; } = (guid ?? GuidProvider.Default).AsSortable.Create().ToString();
    public INode StartingNode { get; } = IsNotNull(start);
    public Context Context { get; } = IsNotNull(context);

    public void Run() {
        var runner = new Runner(this);
        runner.Run();
    }
}
