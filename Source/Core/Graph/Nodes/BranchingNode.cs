namespace DotNetToolbox.Graph.Nodes;

public sealed class BranchingNode(uint id, string label, Func<Context, CancellationToken, Task<string>> select, IServiceProvider services)
    : BranchingNode<BranchingNode>(id, label, services) {
    private const string _defaultLabel = "case";

    private readonly Func<Context, CancellationToken, Task<string>> _select = IsNotNull(select);

    public BranchingNode(uint id, Func<Context, CancellationToken, Task<string>> select, IServiceProvider services)
        : this(id, _defaultLabel, select, services) {
    }
    public BranchingNode(uint id, string label, Func<Context, string> select, IServiceProvider services)
        : this(id, label, (ctx, ct) => Task.Run(() => select(ctx), ct), services) {
    }
    public BranchingNode(uint id, Func<Context, string> select, IServiceProvider services)
        : this(id, _defaultLabel, select, services) {
    }

    protected override Task<string> Select(Context context, CancellationToken ct) => _select(context, ct);

    public static TNode Create<TNode>(uint id, string label, IServiceProvider services)
        where TNode : BranchingNode<TNode>
        => InstanceFactory.Create<TNode>(id, label, services);

    public static TNode Create<TNode>(uint id, IServiceProvider services)
        where TNode : BranchingNode<TNode>
        => InstanceFactory.Create<TNode>(id, services);
}

public abstract class BranchingNode<TNode>
    : Node<TNode>,
      IBranchingNode
    where TNode : BranchingNode<TNode> {
    protected BranchingNode(uint id, string label, IServiceProvider services)
        : base(id, label, services) { }

    protected BranchingNode(uint id, IServiceProvider services)
        : base(id, services) {
    }

    public Dictionary<string, INode?> Choices { get; } = [];

    protected override Result IsValid(ISet<INode> visited) {
        var result = base.IsValid(visited);
        var choices = Choices.Values
                             .Where(c => c is not null)
                             .Cast<INode>()
                             .Distinct();
        return choices.Aggregate(result, ValidateChoice);

        Result ValidateChoice(Result current, INode choice)
            => current + choice.Validate(visited);
    }

    protected override async Task<INode?> GetNext(Context context, CancellationToken ct) {
        ct.ThrowIfCancellationRequested();
        var key = await Select(context, ct);
        var choice = Choices.GetValueOrDefault(key)
            ?? throw new InvalidOperationException($"The path '{key}' was not found.");
        return await choice.Run(context, ct);
    }

    protected abstract Task<string> Select(Context context, CancellationToken ct);

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;
}
