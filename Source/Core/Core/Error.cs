namespace DotNetToolbox;

public record Error {
    public Error(string message, params IReadOnlyList<string> sources) {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Message = message.Trim();
        Sources = [..sources.Where(static i => !string.IsNullOrWhiteSpace(i))
                            .Select(static i => i.Trim())];
    }

    public string Message { get; }
    public IReadOnlyList<string> Sources { get; }

    public static implicit operator Error(string message) => new(message);

    public void Deconstruct(out string message, out IReadOnlyList<string>? sources) {
        message = Message;
        sources = Sources;
    }

    public virtual bool Equals(Error? other)
        => other is not null
        && other.Message == Message
        && other.Sources.SequenceEqual(Sources);

    public override int GetHashCode()
        => HashCode.Combine(Message.GetHashCode(), Sources.Aggregate(0, static (s, i) => HashCode.Combine(s, i.GetHashCode())));

    public override string ToString()
        => $"{{Message: {Message}, Sources: [{string.Join(", ", Sources)}]}}";
}

public record Error<TCode>
    : Error
    where TCode : IComparable<TCode> {
    public Error(TCode code, string message, params IReadOnlyList<string> sources)
        : base(message, sources) {
        Code = code;
    }

    public TCode Code { get; init; }

    public void Deconstruct(out TCode code, out string message, out IReadOnlyList<string>? sources) {
        code = Code;
        message = Message;
        sources = Sources;
    }

    public void Deconstruct(out TCode code, out string message) {
        code = Code;
        message = Message;
    }

    public virtual bool Equals(Error<TCode>? other)
        => base.Equals(other)
        && other.Code.Equals(Code);

    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Code.GetHashCode());

    public override string ToString()
        => $"{{Code: {Code}, Message: {Message}, Sources: [{string.Join(", ", Sources)}]}}";
}
