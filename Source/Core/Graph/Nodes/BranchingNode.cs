namespace DotNetToolbox.Graph.Nodes;

public sealed class BranchingNode(uint id, IServiceProvider services, Func<Context, CancellationToken, Task<string>> select, string? tag = null, string? label = null)
    : BranchingNode<BranchingNode>(id, services, tag, label) {
    private readonly Func<Context, CancellationToken, Task<string>> _select = IsNotNull(select);

    public BranchingNode(uint id, IServiceProvider services, Func<Context, string> selector, string? tag = null, string? label = null)
        : this(id, services, (ctx, ct) => Task.Run(() => selector(ctx), ct), tag, label) {
    }

    protected override string DefaultLabel { get; } = "case";

    protected override Task<string> Select(Context context, CancellationToken ct) => _select(context, ct);

    public static TNode Create<TNode>(uint id, string label, IServiceProvider services)
        where TNode : BranchingNode<TNode>
        => InstanceFactory.Create<TNode>(id, label, services);
    public static TNode Create<TNode>(uint id, IServiceProvider services)
        where TNode : BranchingNode<TNode>
        => InstanceFactory.Create<TNode>(id, services);
}

public abstract class BranchingNode<TNode>(uint id, IServiceProvider services, string? tag = null, string? label = null)
    : Node<TNode>(id, services, tag, label),
      IBranchingNode
    where TNode : BranchingNode<TNode> {
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
