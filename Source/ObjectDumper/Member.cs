namespace DotNetToolbox;

internal readonly record struct Member {
    public Member(MemberKind kind, object? name, Type? type, object? value) {
        Kind = kind;
        Name = name;
        Type = type;
        Value = value;
    }

    public MemberKind Kind { get; }
    public object? Name { get; }
    public Type? Type { get; }
    public object? Value { get; }
}
