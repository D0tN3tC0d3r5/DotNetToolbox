namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public abstract class Parameter<TParameter, TValue>
    : Argument<TParameter>
    , IParameter
    where TParameter : Parameter<TParameter, TValue> {
    protected Parameter(ICommand owner, string name, bool isRequired)
        : base(IsNotNull(owner), "Parameter", name) {
        Order = owner.Children.OfType<IParameter>().Count();
        IsRequired = isRequired;
    }

    public int Order { get; }
    public bool IsRequired { get; }

    public async Task<Result> SetValue(string input, CancellationToken ct) {
        try {
            Value = (TValue)Convert.ChangeType(input, typeof(TValue));
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Failed to convert {input} to {type}", input, typeof(TValue));
            return await ErrorTask($"Failed to convert {input} to {typeof(TValue).Name}");
        }

        return await OnRead(ct);
    }

    public object? Value { get; set; }
}
