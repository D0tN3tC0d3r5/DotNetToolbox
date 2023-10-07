namespace System.Validation;

public sealed record ValidationError {
    private readonly string _messageTemplate;

    public ValidationError([StringSyntax(CompositeFormat)] string message, string source)
        : this(message, new object?[] { source }) {
    }

    public ValidationError([StringSyntax(CompositeFormat)] string messageTemplate, params object?[] args) {
        _messageTemplate = IsNotNullOrWhiteSpace(messageTemplate);
        Source = args.Length == 0 ? string.Empty : IsNotNull(args[0]!.ToString()).Trim();
        Arguments = args;
    }

    public object?[] Arguments { get; }

    public string Source { get; init; }

    public string Message => GetErrorMessage(_messageTemplate, Arguments);

    public bool Equals(ValidationError? other)
        => other is not null
           && Source.Equals(other.Source)
           && Message.Equals(other.Message);

    public override int GetHashCode()
        => HashCode.Combine(Message.GetHashCode(), Source.GetHashCode());
}
