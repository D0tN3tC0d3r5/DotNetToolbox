namespace DotNetToolbox.Results;

[DebuggerDisplay("""
                 ValidationError: Message="{Message}, Source="{Source}""
                 """)]
public sealed record ValidationError {
    private readonly string _formattedMessage;
    public const string DefaultErrorMessage = "The value is invalid.";

    public ValidationError()
        : this(DefaultErrorMessage, string.Empty) {
    }

    public ValidationError(string message, string? source = null) {
        Message = IsNotNullOrWhiteSpace(message).Trim();
        Source = DefaultIfNull(source, string.Empty).Trim();
        _formattedMessage = (string.IsNullOrEmpty(Source) ? string.Empty : $"{Source}: ")
                          + Message;
    }

    public string Message { get; }
    public string Source { get; }

    public static implicit operator ValidationError(string message)
        => new(message);

    public bool Equals(ValidationError? other)
        => _formattedMessage.Equals(other?._formattedMessage, StringComparison.Ordinal);

    public override int GetHashCode()
        => _formattedMessage.GetHashCode();

    public override string Describe()
        => _formattedMessage;
}
