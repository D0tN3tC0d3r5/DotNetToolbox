namespace System.Validation;

public readonly struct ValidationError {
    [SetsRequiredMembers]
    public ValidationError(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string template, params object[] args)
        : this() {
        Source = IsNotNull(source).Trim();
        MessageTemplate = IsNotNullOrEmpty(template);
        Arguments = IsNotNull(args);
    }

    [SetsRequiredMembers]
    public ValidationError([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string template, params object[] args)
        : this(string.Empty, template, args) {
    }

    private readonly string? _source;
    public string Source {
        get => _source ?? string.Empty;
        init => _source = value;
    }

    private readonly string? _messageTemplate;
    public required string MessageTemplate {
        get => _messageTemplate ?? string.Empty;
        init => _messageTemplate = value;
    }

    private readonly object[]? _arguments;
    public object[] Arguments {
        get => _arguments ?? [];
        init => _arguments = value;
    }

    //public string FormattedMessage
    //    => $"{(string.IsNullOrEmpty(Source) ? string.Empty : $"{Source}: ")}{string.Format($"{MessageTemplate}", Arguments ?? Array.Empty<object>())}";
    public string FormattedMessage {
        get {
            var source = string.IsNullOrEmpty(Source) ? string.Empty : $"{Source}: ";
            var message = string.Format(MessageTemplate, Arguments);
            return $"{source}{message}";
        }
    }

    public override bool Equals(object? other)
        => other is ValidationError ve && Equals(ve);

    public static bool operator ==(ValidationError left, object? right)
        => left.Equals(right);

    public static bool operator !=(ValidationError left, object? right)
        => !left.Equals(right);

    public bool Equals(ValidationError other)
        => FormattedMessage.Equals(other.FormattedMessage);

    public override int GetHashCode()
        => FormattedMessage.GetHashCode();
}
