namespace DotNetToolbox.Results;

[DebuggerDisplay("""
                 ValidationError: Message="{Message}, Source="{Source}", Exception="{Exception}"
                 """)]
public readonly struct ValidationError {
    private readonly string _formattedMessage = string.Empty;

    [SetsRequiredMembers]
    public ValidationError(string source, string message, Exception? exception = null)
        : this() {
        Message = IsNotNullOrWhiteSpace(message).Trim();
        Source = IsNotNull(source, string.Empty).Trim();
        Exception = exception;
        _formattedMessage = (string.IsNullOrEmpty(Source) ? string.Empty : $"{Source}: ")
                          + Message
                          + (Exception is null ? string.Empty : $"; {Exception} ");
    }

    [SetsRequiredMembers]
    public ValidationError(string message, Exception? exception = null)
        : this(string.Empty, message, exception) {
    }

    public string Message { get; }
    public string Source { get; }
    public Exception? Exception { get; }

    public static implicit operator ValidationError(Exception innerException)
        => new(innerException.Message, innerException);

    public static implicit operator ValidationError(string message)
        => new(message);

    public bool Equals(ValidationError other)
        => _formattedMessage.Equals(other._formattedMessage);

    public override bool Equals(object? other)
        => other is ValidationError ve && Equals(ve);

    public static bool operator ==(ValidationError left, object? right)
        => left.Equals(right);

    public static bool operator !=(ValidationError left, object? right)
        => !left.Equals(right);

    public override int GetHashCode()
        => _formattedMessage.GetHashCode();

    public override string ToString()
        => _formattedMessage;
}
