namespace DotNetToolbox.ConsoleApplication.Questions;

public record AnswerOption {
    public string Value { get; }
    public string? Alias { get; }
    public string? Text { get; }

    public AnswerOption(string value, string? alias = null, string? text = null) {
        Value = value;
        Alias = alias;
        Text = GetFormattedText();
        return;

        string GetFormattedText() {
            if (text != null) return text;
            if (alias == null) return value;
            var index = value.IndexOf(alias, StringComparison.Ordinal);
            return index == -1
                       ? $"{value} | {alias}"
                       : $"{value[..index]}[{alias}]{value[(index + alias.Length)..]}";
        }
    }

    public bool Matches(string input)
        => Value.Equals(input, StringComparison.InvariantCultureIgnoreCase)
        || Alias is null
        || Alias.Equals(input, StringComparison.InvariantCultureIgnoreCase);
}
