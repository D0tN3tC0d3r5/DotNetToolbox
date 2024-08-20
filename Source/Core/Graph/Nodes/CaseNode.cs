namespace DotNetToolbox.Graph.Nodes;

public sealed class CaseNode : CaseNode<CaseNode> {
    public CaseNode(Func<Context, CancellationToken, Task<string>> select, IServiceProvider services)
        : base(services) {
        _select = IsNotNull(select);
        Label = "case";
    }
    public CaseNode(string tag, Func<Context, CancellationToken, Task<string>> select, IServiceProvider services)
        : this(select, services) {
        Tag = IsNotNullOrWhiteSpace(tag);
    }
    public CaseNode(Func<Context, string> select, IServiceProvider services)
        : this((ctx, ct) => Task.Run(() => select(ctx), ct), services) {
    }
    public CaseNode(string tag, Func<Context, string> select, IServiceProvider services)
        : this(tag, (ctx, ct) => Task.Run(() => select(ctx), ct), services) {
    }

    private readonly Func<Context, CancellationToken, Task<string>> _select;

    protected override Task<string> Select(Context context, CancellationToken ct = default) => _select(context, ct);
}

public abstract class CaseNode<TNode>(IServiceProvider services)
    : Node<TNode>(services),
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

    protected override async Task<INode?> SelectPath(Context context, CancellationToken ct = default) {
        ct.ThrowIfCancellationRequested();
        var key = await Select(context, ct);
        var choice = Choices.GetValueOrDefault(key)
            ?? Choices.GetValueOrDefault(string.Empty)
            ?? throw new InvalidOperationException($"The path '{key}' was not found.");
        return await choice.Run(context, ct);
    }

    protected abstract Task<string> Select(Context context, CancellationToken ct = default);

    protected sealed override Task UpdateState(Context context, CancellationToken ct = default)
        => Task.CompletedTask;

    public sealed override void ConnectTo(INode? next) {
        foreach (var choice in Choices)
            choice.Value?.ConnectTo(next);
    }
}
