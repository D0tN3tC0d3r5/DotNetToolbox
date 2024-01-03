namespace DotNetToolbox.Validation;

public readonly struct ValidationError {
    [SetsRequiredMembers]
    public ValidationError(string source, string message)
        : this() {
        Source = IsNotNull(source).Trim();
        Message = IsNotNullOrEmpty(message);
    }

    [SetsRequiredMembers]
    public ValidationError(string message)
        : this(string.Empty, message) {
    }

    private readonly string? _source;
    public string Source {
        get => _source ?? string.Empty;
        init => _source = value;
    }

    private readonly string? _messageTemplate;
    public required string Message {
        get => _messageTemplate ?? string.Empty;
        init => _messageTemplate = value;
    }

    public static implicit operator ValidationError(string message)
        => new(message);

    public override bool Equals(object? other)
        => other is ValidationError ve && Equals(ve);

    public static bool operator ==(ValidationError left, object? right)
        => left.Equals(right);

    public static bool operator !=(ValidationError left, object? right)
        => !left.Equals(right);

    public bool Equals(ValidationError other)
        => Source.Equals(other.Source) && Message.Equals(other.Message);

    public override int GetHashCode()
        => HashCode.Combine(Source, Message);
}
