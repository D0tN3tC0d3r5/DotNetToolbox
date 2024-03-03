namespace DotNetToolbox.ConsoleApplication.Questions;

public record Choice<TResult> {
    private readonly string _text;
    private readonly string? _order;
    private readonly string? _alias;

    public Choice(TResult result, string text, string? alias = null)
        : this(null, result, text, alias) {
    }
    public Choice(int order, TResult result, string text, string? alias = null)
        : this((order + 1).ToString(), result, text, alias) {
    }

    private Choice(string? order, TResult value, string text, string? alias = null) {
        Value = value;
        _text = text;
        _order = order;
        _alias = alias;
        Display = GetFormattedText();
        return;

        string GetFormattedText() {
            var sb = new StringBuilder();
            if (order is not null) sb.Append($"[{order}]. ");
            var pos = IsNotNullOrWhiteSpace(text).IndexOf(alias ?? string.Empty, StringComparison.OrdinalIgnoreCase);
            if ((alias is not null) && pos != -1) sb.Append(text[..pos]).Append($"[{alias}]").Append(text[(pos + alias.Length)..]);
            else if (alias is not null) sb.Append($"[{alias}] ").Append(text);
            else sb.Append(text);
            return sb.ToString();
        }
    }

    public string Display { get; }
    public TResult Value { get; }

    public bool Matches(string input)
        => _alias?.Equals(input, StringComparison.InvariantCultureIgnoreCase)
        ?? _order?.Equals(input, StringComparison.InvariantCultureIgnoreCase)
        ?? _text.Equals(input, StringComparison.InvariantCultureIgnoreCase);
}
