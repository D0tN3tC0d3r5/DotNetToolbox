using System.Diagnostics;

namespace DotNetToolbox.Results;

[DebuggerDisplay("""
                 ValidationError: Source="{Source}", Message="{Message}"
                 """)]
public readonly struct ValidationError {
    private readonly string _message;
    private readonly string _formattedMessage = string.Empty;
    private readonly string? _source;

    [SetsRequiredMembers]
    public ValidationError(string source, string message)
        : this() {
        _source = IsNotNull(source).Trim();
        _message = IsNotNullOrEmpty(message).Trim();
        _formattedMessage = $"{(string.IsNullOrEmpty(_source) ? string.Empty : $"{_source}: ")}{_message}";
    }

    [SetsRequiredMembers]
    public ValidationError(string message)
        : this(string.Empty, message) {
    }

    public string Source => _source ?? string.Empty;

    public string Message => _message ?? string.Empty;

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
