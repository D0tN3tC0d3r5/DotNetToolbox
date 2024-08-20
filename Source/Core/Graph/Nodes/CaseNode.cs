namespace DotNetToolbox.Graph.Nodes;

public sealed class CaseNode : CaseNode<CaseNode> {
    internal CaseNode(string? id, INodeSequence? sequence, Func<Context, CancellationToken, Task<string>> select)
        : base(id, sequence) {
        _select = IsNotNull(select);
    }

    public CaseNode(string id, Func<Context, CancellationToken, Task<string>> select)
        : this(IsNotNullOrWhiteSpace(id), null, select) {
    }
    public CaseNode(Func<Context, CancellationToken, Task<string>> select, INodeSequence? sequence = null)
        : this(null, sequence, select) {
    }
    public CaseNode(string id, Func<Context, string> select)
        : this(IsNotNullOrWhiteSpace(id), null, (ctx, ct) => Task.Run(() => select(ctx), ct)) {
    }
    public CaseNode(Func<Context, string> select, INodeSequence? sequence = null)
        : this(null, sequence, (ctx, ct) => Task.Run(() => select(ctx), ct)) {
    }

    private readonly Func<Context, CancellationToken, Task<string>> _select;

    protected override string DefaultLabel { get; } = "case";

    protected override Task<string> Select(Context context, CancellationToken ct) => _select(context, ct);
}

public abstract class CaseNode<TNode>(string? id, INodeSequence? sequence)
    : Node<TNode>(id, sequence),
      ICaseNode
    where TNode : CaseNode<TNode> {
    public Dictionary<string, INode?> Choices { get; } = [];

    protected override Result IsValid(ISet<INode> visited) {
        var result = base.IsValid(visited);
        if (Choices.Count == 0)
            result += new ValidationError("The case node has no choices.", Token?.ToSource());
        var choices = Choices.Values
                             .Where(c => c is not null)
                             .Cast<INode>()
                             .Distinct();
        return choices.Aggregate(result, ValidateChoice);

        Result ValidateChoice(Result current, INode choice)
            => current + choice.Validate(visited);
    }

    protected override async Task<INode?> SelectPath(Context context, CancellationToken ct) {
        ct.ThrowIfCancellationRequested();
        var key = await Select(context, ct);
        var choice = Choices.GetValueOrDefault(key)
            ?? Choices.GetValueOrDefault(string.Empty)
            ?? throw new InvalidOperationException($"The path '{key}' was not found.");
        return await choice.Run(context, ct);
    }

    protected abstract Task<string> Select(Context context, CancellationToken ct);

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    public sealed override void ConnectTo(INode? next) {
        foreach (var choice in Choices)
            choice.Value?.ConnectTo(next);
    }
}
