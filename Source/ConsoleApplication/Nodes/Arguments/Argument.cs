using DotNetToolbox.ConsoleApplication.Nodes;

namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public abstract class Argument<TArgument>
    : IArgument
    where TArgument : Argument<TArgument> {
    protected Argument(IExecutableNode parent, ArgumentType type, string name, ILoggerFactory loggerFactory) {
        Logger = loggerFactory.CreateLogger<TArgument>();
        Parent = parent;
        ArgumentType = type;
        Name = name;
    }

    public ILogger<TArgument> Logger { get; }
    public IExecutableNode? Parent { get; }
    ICollection<INamedNode> INode.Children { get; } = [];
    public ArgumentType ArgumentType { get; }
    public string Name { get; }
    public string? Alias { get; init; }
    public string? Description { get; init; }
    public string[] Ids => Alias is null ? [Name] : [Name, Alias];

    protected abstract Task<Result> OnRead(CancellationToken ct);

    public override string ToString() {
        var type = ArgumentType.ToString().PadRight(8);
        var name = $"'--{Name}'".PadRight(24);
        var alias = (Alias is null ? string.Empty : $"'-{Alias}'").PadRight(8);
        var description = string.Join($"{Environment.NewLine}{new string(' ', 40)}", (Description ?? string.Empty).Split(Environment.NewLine).SelectMany(i => i.Chunk(40)).Select(i => string.Join(string.Empty, i)));
        return $"{type}{name}{alias}{description}";
    }
}
