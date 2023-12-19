namespace DotNetToolbox;

internal record Member(object? Name, Type? Type, object? Value) {
    public static readonly Member Default = new(null, null, null);
    public bool IsEnumerable => Value is IEnumerable;
}
