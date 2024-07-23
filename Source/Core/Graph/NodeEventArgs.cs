namespace DotNetToolbox.Graph;

public class NodeEventArgs(Context context, INode? node)
    : EventArgs {
    public INode? Node { get; } = node;
    public Context Context { get; } = context;
    public bool Continue { get; set; } = true;
}
