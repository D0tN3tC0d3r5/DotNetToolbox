namespace DotNetToolbox.Graph.Nodes;

public sealed class CaseNode(IServiceProvider services, uint id, Func<Context, CancellationToken, Task<string>> select, string? tag = null, string? label = null)
    : CaseNode<CaseNode>(services, id, tag, label) {
    private readonly Func<Context, CancellationToken, Task<string>> _select = IsNotNull(select);

    public CaseNode(IServiceProvider services, uint id, Func<Context, string> selector, string? tag = null, string? label = null)
        : this(services, id, (ctx, ct) => Task.Run(() => selector(ctx), ct), tag, label) {
    }

    protected override string DefaultLabel { get; } = "case";

    protected override Task<string> Select(Context context, CancellationToken ct) => _select(context, ct);

    public static TNode Create<TNode>(uint id, string label, IServiceProvider services)
        where TNode : CaseNode<TNode>
        => InstanceFactory.Create<TNode>(id, label, services);
    public static TNode Create<TNode>(uint id, IServiceProvider services)
        where TNode : CaseNode<TNode>
        => InstanceFactory.Create<TNode>(id, services);
}

public abstract class CaseNode<TNode>(IServiceProvider services, uint id, string? tag = null, string? label = null)
    : Node<TNode>(services, id, tag, label),
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
