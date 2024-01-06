using System.Diagnostics;

namespace DotNetToolbox.Validation;

[DebuggerDisplay("""
                 ValidationError: Source="{Source}", Message="{Message}"
                 """)]
public readonly struct ValidationError {
    private readonly string _message;

    [SetsRequiredMembers]
    public ValidationError(string source, string message)
        : this() {
        Source = IsNotNull(source).Trim();
        Message = IsNotNullOrWhiteSpace(message).Trim();
    }

    [SetsRequiredMembers]
    public ValidationError(string message)
        : this(string.Empty, message) {
    }

    public string Source { get; } = string.Empty;

    public required string Message {
        get => _message;
        [MemberNotNull(nameof(_message))]
        init => _message = IsNotNullOrWhiteSpace(value).Trim();
    }

    public static implicit operator ValidationError(string message)
        => new(message);

    public bool Equals(ValidationError other)
        => Source.Equals(other.Source) && Message.Equals(other.Message);

    public override bool Equals(object? other)
        => other is ValidationError ve && Equals(ve);

    public static bool operator ==(ValidationError left, object? right)
        => left.Equals(right);

    public static bool operator !=(ValidationError left, object? right)
        => !left.Equals(right);

    public override int GetHashCode()
        => HashCode.Combine(Source, Message);

    public override string ToString()
        => $"{Source}{(Source == string.Empty ? string.Empty : ": ")}{Message}";
}
