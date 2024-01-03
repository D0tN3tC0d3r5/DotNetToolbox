using DotNetToolbox.ConsoleApplication.Nodes.Commands;

namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public abstract class Option<TOption, TValue>
    : Argument<TOption>
    , IOption
    where TOption : Option<TOption, TValue> {
    protected Option(ICommand owner, ArgumentType type, string name, ILoggerFactory loggerFactory)
        : base(IsNotNull(owner), type, name, loggerFactory) {
    }

    public TValue Value { get; private set; } = default!;
    public async Task<Result> SetValue(string input, CancellationToken ct) {
        try {
            Value = (TValue)Convert.ChangeType(input, typeof(TValue));
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Failed to convert {input} to {type}", input, typeof(TValue).Name);
            return await Result.ErrorTask($"Failed to convert {input} to {typeof(TValue).Name}");
        }

        return await OnRead(ct);
    }
}
