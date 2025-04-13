namespace WebApi.Tokens;

/// <inheritdoc />
public abstract record Token()
    : IToken {
    [SetsRequiredMembers]
    protected Token(string type, string value)
        : this() {
        Type = type;
        Id = Guid.CreateVersion7();
        Value = Ensure.IsNotNullOrWhiteSpace(value);
    }

    /// <inheritdoc />
    public required Guid Id { get; init; }
    /// <inheritdoc />
    public string Type { get; } = string.Empty;
    /// <inheritdoc />
    public required string Value { get; init; }
}