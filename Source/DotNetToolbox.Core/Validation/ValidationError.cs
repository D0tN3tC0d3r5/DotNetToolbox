namespace System.Validation;

public sealed record ValidationError {
    public ValidationError(string? source, [StringSyntax(CompositeFormat)] string template, params object[] args) {
        MessageTemplate = IsNotNullOrWhiteSpace(template);
        Arguments = args ?? Array.Empty<object>();
        Source = source ?? string.Empty;
    }

    public ValidationError([StringSyntax(CompositeFormat)] string template, params object[] args)
        : this(string.Empty, template, args) {
    }

    public string Source { get; set; }
    public string MessageTemplate { get; set; }
    public object[] Arguments { get; set; }
    public string FormattedMessage
        => (string.IsNullOrWhiteSpace(Source) ? string.Empty : $"{Source}: ")
        + string.Format(MessageTemplate, Arguments);

    public bool Equals(ValidationError? other)
        => other is not null
        && Source.Equals(other.Source)
        && MessageTemplate.Equals(other.MessageTemplate)
        && Arguments.SequenceEqual(other.Arguments);

    public override int GetHashCode()
        => Arguments.Aggregate(HashCode.Combine(Source, MessageTemplate), HashCode.Combine);
}
