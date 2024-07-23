namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeFactory(IKeyProvider<uint>? key = null)
    : INodeFactory {
    private readonly IKeyProvider<uint> _key = key ?? new SequentialKeyProvider();

    public string GenerateId() => _key.GetNext().ToString();

    public IConditionalNode If(Func<Context, bool> predicate, Action<WorkflowBuilder> setTruePath, Action<WorkflowBuilder>? setFalsePath = null)
        => ConditionalNode.Create(predicate, setTruePath, setFalsePath, this);

    public IBranchingNode Select(Func<Context, string> selectPath, Action<BranchesBuilder> setPaths)
        => BranchingNode.Create(selectPath, setPaths, this);

    public IActionNode Do(Action<Context> action, Action<WorkflowBuilder>? setNext = null, IPolicy? policy = null)
        => ActionNode.Create(action, setNext, policy, this);

    public IActionNode Do<TAction>()
        where TAction : ActionNode<TAction>
        => ActionNode.Create<TAction>(this);

    public INode Start
        => EntryNode.Create(this);

    public INode End
        => ExitNode.Create(this);
}
