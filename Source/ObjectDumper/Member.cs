namespace DotNetToolbox;

internal record struct Member(MemberKind Kind, object? Name, Type? Type, object? Value);
