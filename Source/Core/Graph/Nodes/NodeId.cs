namespace DotNetToolbox.Graph.Nodes;

public static class NodeId {
    private static readonly IKeyProvider<uint> _key = new SequentialKeyProvider();
    internal static uint GetNext() => _key.GetNext();
    internal static void Reset() => _key.Reset();
}
