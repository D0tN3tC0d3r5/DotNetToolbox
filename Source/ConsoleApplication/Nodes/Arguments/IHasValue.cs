namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public interface IHasValue<out TValue> : IHasValue {
    public TValue? Value { get; }
}

public interface IHasValue {
    Task<Result> SetValue(string input, CancellationToken ct);
}
