using DotNetToolbox.ConsoleApplication.Nodes.Commands;

namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public abstract class Flag<TFlag>
    : Argument<TFlag>
    , IFlag
    where TFlag : Flag<TFlag> {
    protected Flag(ICommand owner, ArgumentType type, string name, ILoggerFactory loggerFactory)
        : base(IsNotNull(owner), type, name, loggerFactory) {
    }

    public bool IsSet { get; private set; }
    public Task<Result> SetValue(string input, CancellationToken ct) {
        IsSet = true;
        return OnRead(ct);
    }
}
