namespace DotNetToolbox.Graph.Nodes;

public sealed class CaseNode(string id, Func<Context, CancellationToken, Task<string>> select, INodeSequence? sequence = null)
    : CaseNode<CaseNode>(id, sequence) {
    public CaseNode(string id, Func<Context, string> select, INodeSequence? sequence = null)
        : this(id, (ctx, ct) => Task.Run(() => select(ctx), ct), sequence) {
    }
    public CaseNode(Func<Context, CancellationToken, Task<string>> select, INodeSequence? sequence = null)
        : this(null!, select, sequence) {
    }
    public CaseNode(Func<Context, string> select, INodeSequence? sequence = null)
        : this(null!, (ctx, ct) => Task.Run(() => select(ctx), ct), sequence) {
    }

    private readonly Func<Context, CancellationToken, Task<string>> _select = IsNotNull(select);

    protected override string DefaultLabel { get; } = "case";

    protected override Task<string> Select(Context context, CancellationToken ct) => _select(context, ct);

    public static TNode Create<TNode>(IServiceProvider services, string? id = null)
        where TNode : CaseNode<TNode>
        => Node.Create<TNode>(services, id);

    public static TNode Create<TNode>(string? id = null, INodeSequence? sequence = null, params object?[] args)
        where TNode : CaseNode<TNode>
        => Node.Create<TNode>(id ?? string.Empty, sequence, args);
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
            ?? throw new InvalidOperationException($"The path '{key}' was not found.");
        return await choice.Run(context, ct);
    }

    protected abstract Task<string> Select(Context context, CancellationToken ct);

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    public sealed override Result ConnectTo(INode? next) {
        var result = Success();
        var token = next?.Token;
        if (token?.Type is TokenType.Is) {
            var key = token.Value;
            if (string.IsNullOrEmpty(key))
                result += new ValidationError("The case key cannot be empty.", token.ToSource());
            else if (!Choices.TryAdd(key, next))
                result += new ValidationError($"The case choice '{key}' is already set.", token.ToSource());
        }
        else if (token?.Type is TokenType.Otherwise) {
            if (!Choices.TryAdd(string.Empty, next))
                result += new ValidationError($"The default case is already set.", token.ToSource());
        }
        else {
            if (Choices.Count == 0)
                result += new ValidationError("The case node has no choices.", Token?.ToSource());
            foreach (var choice in Choices) {
                if (choice.Value is null) Choices[choice.Key] = next;
                else result += choice.Value.ConnectTo(next);
            }
        }
        return result;
    }
}
